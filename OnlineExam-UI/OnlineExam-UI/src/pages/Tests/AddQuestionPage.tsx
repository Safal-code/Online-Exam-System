import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import Input from "../../components/common/Input";
import Button from "../../components/common/Button";
import ErrorMessage from "../../components/common/ErrorMessage";

import { createQuestion, getQuestions } from "../../services/questionService";

import { useAuth } from "../../hooks/useAuth";

interface OptionForm {
  optionText: string;
  isCorrect: boolean;
}

export default function AddQuestionPage() {
  const { testId } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();

  const [questionText, setQuestionText] = useState("");

  const [options, setOptions] = useState<OptionForm[]>([
    {
      optionText: "",
      isCorrect: true,
    },
    {
      optionText: "",
      isCorrect: false,
    },
  ]);

  const [error, setError] = useState("");

  function updateOption(index: number, text: string) {
    setOptions((current) =>
      current.map((option, optionIndex) =>
        optionIndex === index
          ? {
              ...option,
              optionText: text,
            }
          : option,
      ),
    );
  }

  function setCorrectOption(index: number) {
    setOptions((current) =>
      current.map((option, optionIndex) => ({
        ...option,
        isCorrect: optionIndex === index,
      })),
    );
  }

  function addOption() {
    setOptions((current) => [
      ...current,
      {
        optionText: "",
        isCorrect: false,
      },
    ]);
  }

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!user || !testId) return;

    if (options.length < 2) {
      setError("At least 2 options are required.");
      return;
    }

    if (!options.some((option) => option.isCorrect)) {
      setError("Select a correct option.");
      return;
    }

    try {
      /*
       * Get existing questions so we can
       * calculate the next question order.
       */
      const existingQuestions = await getQuestions(Number(testId), user.userId);

      const nextQuestionOrder =
        existingQuestions.length === 0
          ? 1
          : Math.max(
              ...existingQuestions.map((question) => question.questionOrder),
            ) + 1;

      await createQuestion(
        Number(testId),
        {
          questionText,
          questionOrder: nextQuestionOrder,

          options: options.map((option, index) => ({
            optionText: option.optionText,

            isCorrect: option.isCorrect,

            optionOrder: index + 1,
          })),
        },
        user.userId,
      );

      //reset the state after i ad quest
      setQuestionText("");
      setOptions([
        { optionText: "", isCorrect: true },
        { optionText: "", isCorrect: false },
      ]);
      setError("");

      //  * Stay on the same page so admin
      //  * can immediately add another question.

      navigate(`/tests/${testId}/questions/add`);
    } catch (error) {
      setError(
        error instanceof Error ? error.message : "Failed to create question",
      );
    }
  }

  return (
    <main className="page">
      <div className="card form-card">
        <h1>Add Question</h1>

        {error && <ErrorMessage message={error} />}

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Question</label>

            <textarea
              className="input"
              value={questionText}
              onChange={(e) => setQuestionText(e.target.value)}
              required
            />
          </div>

          <h3>Options</h3>

          {options.map((option, index) => (
            <div className="option-form" key={index}>
              <Input
                label={`Option ${index + 1}`}
                value={option.optionText}
                onChange={(e) => updateOption(index, e.target.value)}
                required
              />

              <label>
                <input
                  type="radio"
                  name="correctOption"
                  checked={option.isCorrect}
                  onChange={() => setCorrectOption(index)}
                />
                Correct answer
              </label>
            </div>
          ))}

          <Button type="button" onClick={addOption}>
            Add Option
          </Button>

          <Button type="submit">Save Question</Button>
        </form>
      </div>
    </main>
  );
}
