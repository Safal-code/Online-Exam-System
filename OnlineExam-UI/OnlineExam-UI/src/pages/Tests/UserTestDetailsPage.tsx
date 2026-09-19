import { useEffect, useState } from "react";
import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { useAuth } from "../../hooks/useAuth";
import { getPublishedTests } from "../../services/testService";

import type { PublishedTest } from "../../types/test";

import Loader from "../../components/common/Loader";
import ErrorMessage from "../../components/common/ErrorMessage";
import Button from "../../components/common/Button";

export default function UserTestDetailsPage() {
  const { testId } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();

  const [test, setTest] =
    useState<PublishedTest | null>(null);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!user || !testId) return;

    async function loadTest() {
      try {
        const tests = await getPublishedTests(
          user.userId
        );

        const found = tests.find(
          (item) =>
            item.testId === Number(testId)
        );

        setTest(found ?? null);
      } catch (error) {
        setError(
          error instanceof Error
            ? error.message
            : "Failed to load test"
        );
      } finally {
        setLoading(false);
      }
    }

    loadTest();
  }, [testId, user]);

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
      {error && <ErrorMessage message={error} />}

      <div className="card">
        <h1>{test.name}</h1>

        <p>
          <strong>Subject:</strong>{" "}
          {test.subject}
        </p>

        <p>{test.description}</p>

        <p>
          <strong>Duration:</strong>{" "}
          {test.durationMinutes} minutes
        </p>

        <p>
          <strong>Passing:</strong>{" "}
          {test.passingPercentage}%
        </p>

        <Button
          onClick={() =>
            navigate(`/exam/${test.testId}`)
          }
        >
          Start Test
        </Button>
      </div>
    </main>
  );
}