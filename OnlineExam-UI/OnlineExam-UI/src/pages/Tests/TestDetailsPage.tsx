//this page used by admin to mangage test
import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";

import { useAuth } from "../../hooks/useAuth";
import { getQuestions } from "../../services/questionService";

import { getTests, publishTest, deleteTest } from "../../services/testService";

import type { Test } from "../../types/test";
import type { Question } from "../../types/question";

import Loader from "../../components/common/Loader";
import ErrorMessage from "../../components/common/ErrorMessage";
import Button from "../../components/common/Button";

export default function TestDetailsPage() {
  const { testId } = useParams();
  const { user } = useAuth();
  const navigate = useNavigate();

  const [test, setTest] = useState<Test | null>(null);
  const [questions, setQuestions] = useState<Question[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  async function loadData() {
    if (!user || !testId) return;

    try {
      const tests = await getTests(user.userId);

      const foundTest = tests.find((item) => item.testId === Number(testId));

      setTest(foundTest ?? null);

      const questionData = await getQuestions(Number(testId), user.userId);

      setQuestions(questionData);
    } catch (error) {
      setError(error instanceof Error ? error.message : "Failed to load test");
    } finally {
      setLoading(false);
    }
  }

  //handle test delete
  async function handleDelete() {
    if (!user || !testId) return;

    const confirmed = window.confirm(
      "Are you sure you want to delete this test?",
    );

    if (!confirmed) return;

    try {
      await deleteTest(Number(testId), user.userId);

      navigate("/dashboard");
    } catch (error) {
      setError(
        error instanceof Error ? error.message : "Failed to delete test",
      );
    }
  }

  useEffect(() => {
    loadData();
  }, [testId, user]);

  async function handlePublish() {
    if (!user || !testId) return;

    try {
      await publishTest(Number(testId), user.userId);

      await loadData();
    } catch (error) {
      setError(
        error instanceof Error ? error.message : "Failed to publish test",
      );
    }
  }

  if (loading) return <Loader />;

  if (!test) {
    return (
      <main className="page">
        <p>Test not found.</p>
      </main>
    );
  }

  return (
    <main className="page">
      <div className="page-header">
        <div>
          <h1>{test.name}</h1>
          <p>{test.subject}</p>
        </div>

        {!test.isPublished && (
          <div>
            <Link className="btn" to={`/tests/${test.testId}/questions/add`}>
              Add Question
            </Link>

            <Button onClick={handlePublish}>Publish Test</Button>
          </div>
        )}

        <Button onClick={handleDelete}>Delete Test</Button>
      </div>

      {error && <ErrorMessage message={error} />}

      <div className="card">
        <p>{test.description}</p>

        <p>Duration: {test.durationMinutes} minutes</p>

        <p>Passing: {test.passingPercentage}%</p>

        <p>Status: {test.isPublished ? "Published" : "Draft"}</p>
      </div>

      <h2>Questions</h2>

      {questions.map((question) => (
        <div className="card" key={question.questionId}>
          <h3>
            {question.questionOrder}. {question.questionText}
          </h3>

          <ul>
            {question.options.map((option) => (
              <li key={option.optionId}>{option.optionText}</li>
            ))}
          </ul>
        </div>
      ))}
    </main>
  );
}
