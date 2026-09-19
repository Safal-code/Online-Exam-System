import { apiClient } from "../api/apiClient";
import type {
  CreateQuestionRequest,
  Question,
} from "../types/question";

export function createQuestion(
  testId: number,
  request: CreateQuestionRequest,
  userId: number
): Promise<Question> {
  return apiClient<Question>(`/tests/${testId}/questions`, {
    method: "POST",
    userId,
    body: JSON.stringify(request),
  });
}

export function getQuestions(
  testId: number,
  userId: number
): Promise<Question[]> {
  return apiClient<Question[]>(
    `/tests/${testId}/questions`,
    {
      userId,
    }
  );
}