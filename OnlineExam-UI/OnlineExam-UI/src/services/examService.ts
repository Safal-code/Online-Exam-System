import { apiClient } from "../api/apiClient";

import type {
  StartExamResult,
  SubmitExamRequest,
} from "../types/exam";

import type { ExamResult } from "../types/result";

export function startExam(
  testId: number,
  userId: number
): Promise<StartExamResult> {
  return apiClient<StartExamResult>(
    `/tests/${testId}/start`,
    {
      method: "POST",
      userId,
    }
  );
}

export function submitExam(
  attemptId: number,
  request: SubmitExamRequest,
  userId: number
): Promise<ExamResult> {
  return apiClient<ExamResult>(
    `/exams/${attemptId}/submit`,
    {
      method: "POST",
      userId,
      body: JSON.stringify(request),
    }
  );
}

export function getResult(
  attemptId: number,
  userId: number
): Promise<ExamResult> {
  return apiClient<ExamResult>(
    `/exams/${attemptId}/result`,
    {
      userId,
    }
  );
}