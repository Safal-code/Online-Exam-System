import type { User } from "./user";

export interface LoginRequest {
  userName: string;
  password: string;
}

export type LoginResponse = User;