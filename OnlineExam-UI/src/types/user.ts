export interface User {
  userId: number;
  userName: string;
  name: string;
  role: "Admin" | "User";
}