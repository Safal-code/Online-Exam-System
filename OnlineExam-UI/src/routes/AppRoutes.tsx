import { Navigate, Route, Routes } from "react-router-dom";

import LoginPage from "../pages/Login/LoginPage";
import RegisterPage from "../pages/Register/RegisterPage";
import HomePage from "../pages/Home/HomePage";
import DashboardPage from "../pages/Dashboard/DashboardPage";

import CreateTestPage from "../pages/Tests/CreateTestPage";
import TestDetailsPage from "../pages/Tests/TestDetailsPage";
import AddQuestionPage from "../pages/Tests/AddQuestionPage";
import UserTestDetailsPage from "../pages/Tests/UserTestDetailsPage";

import ExamPage from "../pages/Exam/ExamPage";
import ResultPage from "../pages/Exam/ResultPage";

import ProfilePage from "../pages/Profile/ProfilePage";

import ProtectedRoute from "./ProtectedRoute";

export default function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />


      {/* User routes */}
      <Route element={<ProtectedRoute role="User" />}>
        <Route path="/" element={<HomePage />} />

        <Route path="/tests/:testId" element={<UserTestDetailsPage />} />

        <Route path="/exam/:testId" element={<ExamPage />} />

        <Route path="/result/:attemptId" element={<ResultPage />} />
      </Route>


      {/* Admin routes */}
      <Route element={<ProtectedRoute role="Admin" />}>

       <Route path="/" element={<Navigate to="/dashboard" replace />} />

        <Route path="/dashboard" element={<DashboardPage />} />

        <Route path="/tests/create" element={<CreateTestPage />} />

        <Route path="/tests/:testId/manage" element={<TestDetailsPage />} />

        <Route
          path="/tests/:testId/questions/add"
          element={<AddQuestionPage />}
        />
      </Route>

      {/* Common */}
      <Route element={<ProtectedRoute />}>
        <Route path="/profile" element={<ProfilePage />} />
      </Route>

      <Route path="*" element={<Navigate to="/login" replace />} />
    </Routes>
  );
}
