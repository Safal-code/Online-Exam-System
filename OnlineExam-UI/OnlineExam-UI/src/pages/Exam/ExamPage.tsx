import { useCallback, useEffect, useRef, useState } from "react";

import { useNavigate, useParams } from "react-router-dom";

import { useAuth } from "../../hooks/useAuth";

import { startExam, submitExam } from "../../services/examService";

import type { StartExamResult, SubmitAnswer } from "../../types/exam";

import Loader from "../../components/common/Loader";
import ErrorMessage from "../../components/common/ErrorMessage";
import Button from "../../components/common/Button";

export default function ExamPage() {
  const { testId } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();

  const [exam, setExam] = useState<StartExamResult | null>(null);

  const [currentIndex, setCurrentIndex] = useState(0);

  const [answers, setAnswers] = useState<Record<number, number | null>>({});

  const [secondsLeft, setSecondsLeft] = useState(0);

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState("");

  /*
   * Prevents multiple submit requests.
   */
  const isSubmitting = useRef(false);

  useEffect(() => {
    if (!user || !testId) return;

    async function loadExam() {
      if (!user) {
        return;
      }
      try {
        const data = await startExam(Number(testId), user.userId);

        setExam(data);

        const expires = new Date(data.expiresAt).getTime();

        const remaining = Math.max(
          0,
          Math.floor((expires - Date.now()) / 1000),
        );

        setSecondsLeft(remaining);
      } catch (error) {
        setError(
          error instanceof Error ? error.message : "Failed to start exam",
        );
      } finally {
        setLoading(false);
      }
    }

    loadExam();
  }, [testId, user]);

  /*
   * Timer.
   */
  useEffect(() => {
    if (!exam || secondsLeft <= 0) {
      return;
    }

    const timer = setInterval(() => {
      setSecondsLeft((current) => Math.max(0, current - 1));
    }, 1000);

    return () => clearInterval(timer);
  }, [exam, secondsLeft]);

  /*
   * Submit the exam.
   */
  const handleSubmit = useCallback(async () => {
    if (!exam || !user || isSubmitting.current) {
      return;
    }

    isSubmitting.current = true;

    try {
      const requestAnswers: SubmitAnswer[] = exam.questions.map((question) => ({
        questionId: question.questionId,

        selectedOptionId: answers[question.questionId] ?? null,
      }));

      const result = await submitExam(
        exam.testAttemptId,
        {
          answers: requestAnswers,
        },
        user.userId,
      );

      navigate(`/result/${result.testAttemptId}`);
    } catch (error) {
      /*
       * Allow another attempt if
       * submission itself failed.
       */
      isSubmitting.current = false;

      setError(
        error instanceof Error ? error.message : "Failed to submit exam",
      );
    }
  }, [exam, user, answers, navigate]);

  /*
   * Automatically submit when timer
   * reaches zero.
   */
  useEffect(() => {
    if (!exam || loading || secondsLeft !== 0 || isSubmitting.current) {
      return;
    }

    void handleSubmit();
  }, [exam, loading, secondsLeft, handleSubmit]);

  function selectAnswer(optionId: number) {
    if (!exam) return;

    const questionId = exam.questions[currentIndex].questionId;

    setAnswers((current) => ({
      ...current,
      [questionId]: optionId,
    }));
  }

  if (loading) {
    return <Loader />;
  }

  if (!exam) {
    return (
      <main className="page">
        <ErrorMessage message={error || "Exam not found"} />
      </main>
    );
  }

  const question = exam.questions[currentIndex];

  const minutes = Math.floor(secondsLeft / 60);

  const seconds = secondsLeft % 60;

  return (
    <main className="page">
      <div className="exam-header">
        <div>
          <h1>{exam.testName}</h1>

          <p>
            Question {currentIndex + 1} of {exam.questions.length}
          </p>
        </div>

        <div className="timer">
          Time: {minutes}:{seconds.toString().padStart(2, "0")}
        </div>
      </div>

      {error && <ErrorMessage message={error} />}

      <div className="card">
        <h2>{question.questionText}</h2>

        <div>
          {question.options.map((option) => (
            <label className="answer-option" key={option.optionId}>
              <input
                type="radio"
                name={`question-${question.questionId}`}
                checked={answers[question.questionId] === option.optionId}
                onChange={() => selectAnswer(option.optionId)}
              />

              {option.optionText}
            </label>
          ))}
        </div>

        <div className="exam-actions">
          <Button
            type="button"
            disabled={currentIndex === 0 || isSubmitting.current}
            onClick={() => setCurrentIndex(currentIndex - 1)}
          >
            Previous
          </Button>

          {currentIndex < exam.questions.length - 1 ? (
            <Button
              type="button"
              disabled={isSubmitting.current}
              onClick={() => setCurrentIndex(currentIndex + 1)}
            >
              Next
            </Button>
          ) : (
            <Button
              type="button"
              disabled={isSubmitting.current}
              onClick={() => void handleSubmit()}
            >
              Submit Exam
            </Button>
          )}
        </div>
      </div>
    </main>
  );
}
