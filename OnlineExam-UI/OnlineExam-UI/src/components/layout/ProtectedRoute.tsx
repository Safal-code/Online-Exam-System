import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";

interface ProtectedRouteProps {
  role?: "Admin" | "User";
}

export default function ProtectedRoute({
  role,
}: ProtectedRouteProps) {
  const { user } = useAuth();

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  if(role && user.role !== role){
    const goTo = user.role === "Admin"? "/dashboard" : "/";
    return <Navigate to={goTo} replace/>;
  }

  return <Outlet />;
}