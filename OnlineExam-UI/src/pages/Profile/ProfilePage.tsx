import { useAuth } from "../../hooks/useAuth";

export default function ProfilePage() {
  const { user } = useAuth();

  return (
    <main className="page">
      <div className="card">
        <h1>Profile</h1>

        <p>
          <strong>Name:</strong>{" "}
          {user?.name}
        </p>

        <p>
          <strong>Username:</strong>{" "}
          {user?.userName}
        </p>

        <p>
          <strong>Role:</strong>{" "}
          {user?.role}
        </p>
      </div>
    </main>
  );
}