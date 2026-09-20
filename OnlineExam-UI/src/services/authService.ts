import { apiClient } from "../api/apiClient";
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest
} from "../types/auth";

export function loginUser(
  request: LoginRequest
): Promise<LoginResponse> {
  return apiClient<LoginResponse>("/auth/login", {
    method: "POST",
    body: JSON.stringify(request),
  });
}

export function registerUser(
  request: RegisterRequest
): Promise<{ message: string }> {
  return apiClient<{ message: string }>(
    "/auth/register",
    {
      method: "POST",
      body: JSON.stringify(request),
    }
  );
}