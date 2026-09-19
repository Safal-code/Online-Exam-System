import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import { useAuth } from "../../hooks/useAuth";
import { getTests } from "../../services/testService";

import type { Test } from "../../types/test";

import Loader from "../../components/common/Loader";
import ErrorMessage from "../../components/common/ErrorMessage";

export default function DashboardPage() {
  const { user } = useAuth();

  const [tests, setTests] = useState<Test[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!user) return;

    async function loadTests() {
      if (!user) {
        return;
      }
      try {
        const data = await getTests(user.userId);
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
      <div className="page-header">
        <div>
          <h1>Admin Dashboard</h1>
          <p>Welcome, {user?.name}</p>
        </div>

        <Link className="btn" to="/tests/create">
          Create Test
        </Link>
      </div>

      {error && <ErrorMessage message={error} />}

      <div className="grid">
        {tests.map((test) => (
          <div className="card" key={test.testId}>
            <h2>{test.name}</h2>

            <p>Subject: {test.subject}</p>

            <p>
              Status:{" "}
              {test.isPublished
                ? "Published"
                : "Draft"}
            </p>

            <p>
              Users Appeared: {test.usersAppeared}
            </p>

            <Link
              className="btn"
              to={`/tests/${test.testId}/manage`}
            >
              Manage Test
            </Link>
          </div>
        ))}
      </div>
    </main>
  );
}