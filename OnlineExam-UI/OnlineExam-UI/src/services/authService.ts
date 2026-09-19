import { apiClient } from "../api/apiClient";
import type {
  LoginRequest,
  LoginResponse,
} from "../types/auth";

export function loginUser(
  request: LoginRequest
): Promise<LoginResponse> {
  return apiClient<LoginResponse>("/auth/login", {
    method: "POST",
    body: JSON.stringify(request),
  });
}

export interface RegisterRequest {
  userName: string;
  name: string;
  password: string;
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