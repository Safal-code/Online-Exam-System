import { useEffect, useState } from "react";
import {
  Link,
  useParams,
} from "react-router-dom";

import { useAuth } from "../../hooks/useAuth";
import { getResult } from "../../services/examService";

import type { ExamResult } from "../../types/result";

import Loader from "../../components/common/Loader";
import ErrorMessage from "../../components/common/ErrorMessage";

export default function ResultPage() {
  const { attemptId } = useParams();
  const { user } = useAuth();

  const [result, setResult] =
    useState<ExamResult | null>(null);

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState("");

  useEffect(() => {
    if (!user || !attemptId) return;

    async function loadResult() {
      if (!user) {
        return;
      }
      try {
        const data = await getResult(
          Number(attemptId),
          user.userId
        );

        setResult(data);
      } catch (error) {
        setError(
          error instanceof Error
            ? error.message
            : "Failed to load result"
        );
      } finally {
        setLoading(false);
      }
    }

    loadResult();
  }, [attemptId, user]);

  if (loading) return <Loader />;

  if (!result) {
    return (
      <main className="page">
        <ErrorMessage
          message={error || "Result not found"}
        />
      </main>
    );
  }

  return (
    <main className="page result-page">
      <div
        className={`result-card ${
          result.isPassed
            ? "passed"
            : "failed"
        }`}
      >
        <h1>
          {result.isPassed
            ? "Passed!"
            : "Failed"}
        </h1>

        <h2>{result.score}%</h2>

        <p>
          Total Questions:{" "}
          {result.totalQuestions}
        </p>

        <Link className="btn" to="/">
          Back to Home
        </Link>
      </div>
    </main>
  );
}