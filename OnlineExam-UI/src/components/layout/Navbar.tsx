import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate("/login");
  }

  return (
    <nav className="navbar">
      <Link to={user?.role === "Admin" ? "/dashboard" : "/"} className="logo">
        Online Exam
      </Link>

      <div className="nav-right">
        {user && (
          <>
            <span>Hello, {user.name}</span>

            <Link to="/profile">Profile</Link>

            {user.role === "Admin" && <Link to="/dashboard">Dashboard</Link>}

            <button onClick={handleLogout}>Logout</button>
          </>
        )}
      </div>
    </nav>
  );
}
