import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import { useAuth } from "../../hooks/useAuth";
import { getPublishedTests } from "../../services/testService";

import type { PublishedTest } from "../../types/test";

import Loader from "../../components/common/Loader";
import ErrorMessage from "../../components/common/ErrorMessage";

export default function HomePage() {
  const { user } = useAuth();

  const [tests, setTests] = useState<PublishedTest[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!user) return;

    async function loadTests() {
      try {
        const data = await getPublishedTests(user.userId);
        setTests(data);
      } catch (error) {
        setError(
          error instanceof Error
            ? error.message
            : "Failed to load tests"
        );
      } finally {
        setLoading(false);
      }
    }

    loadTests();
  }, [user]);

  if (loading) return <Loader />;

  return (
    <main className="page">
      <h1>Available Tests</h1>

      {error && <ErrorMessage message={error} />}

      {tests.length === 0 && (
        <p>No published tests available.</p>
      )}

      <div className="grid">
        {tests.map((test) => (
          <div className="card" key={test.testId}>
            <h2>{test.name}</h2>

            <p>
              <strong>Subject:</strong> {test.subject}
            </p>

            <p>{test.description}</p>

            <p>
              Duration: {test.durationMinutes} minutes
            </p>

            <p>
              Passing: {test.passingPercentage}%
            </p>

            <Link
              className="btn"
              to={`/tests/${test.testId}`}
            >
              View Test
            </Link>
          </div>
        ))}
      </div>
    </main>
  );
}