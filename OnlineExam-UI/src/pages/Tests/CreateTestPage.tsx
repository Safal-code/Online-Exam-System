import { useState } from "react";
import { useNavigate } from "react-router-dom";

import Input from "../../components/common/Input";
import Button from "../../components/common/Button";
import ErrorMessage from "../../components/common/ErrorMessage";

import { createTest } from "../../services/testService";
import { useAuth } from "../../hooks/useAuth";

export default function CreateTestPage() {
  const { user } = useAuth();
  const navigate = useNavigate();

  const [name, setName] = useState("");
  const [subject, setSubject] = useState("");
  const [description, setDescription] = useState("");
  const [duration, setDuration] = useState(30);
  const [passing, setPassing] = useState(50);

  const [error, setError] = useState("");

  async function handleSubmit(
    event: React.FormEvent<HTMLFormElement>
  ) {
    event.preventDefault();

    if (!user) return;

    try {
      const test = await createTest(
        {
          name,
          subject,
          description,
          durationMinutes: duration,
          passingPercentage: passing,
        },
        user.userId
      );

      // After creating test, directly add questions
      navigate(`/tests/${test.testId}/questions/add`);
    } catch (error) {
      setError(
        error instanceof Error
          ? error.message
          : "Failed to create test"
      );
    }
  }

  return (
    <main className="page">
      <div className="card form-card">
        <h1>Create Test</h1>

        {error && <ErrorMessage message={error} />}

        <form onSubmit={handleSubmit}>
          <Input
            label="Test Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />

          <Input
            label="Subject"
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            required
          />

          <div className="form-group">
            <label>Description</label>

            <textarea
              className="input"
              value={description}
              onChange={(e) =>
                setDescription(e.target.value)
              }
            />
          </div>

          <Input
            label="Duration (minutes)"
            type="number"
            min="1"
            value={duration}
            onChange={(e) =>
              setDuration(Number(e.target.value))
            }
            required
          />

          <Input
            label="Passing Percentage"
            type="number"
            min="0"
            max="100"
            value={passing}
            onChange={(e) =>
              setPassing(Number(e.target.value))
            }
            required
          />

          <Button type="submit">
            Create Test
          </Button>
        </form>
      </div>
    </main>
  );
}