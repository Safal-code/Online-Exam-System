import { apiClient } from "../api/apiClient";

import type {
  CreateTestRequest,
  PublishTestResult,
  PublishedTest,
  Test,
} from "../types/test";

export function createTest(
  request: CreateTestRequest,
  userId: number
): Promise<Test> {
  return apiClient<Test>("/tests", {
    method: "POST",
    userId,
    body: JSON.stringify(request),
  });
}

export function getTests(userId: number): Promise<Test[]> {
  return apiClient<Test[]>("/tests", {
    userId,
  });
}

export function getPublishedTests(
  userId: number
): Promise<PublishedTest[]> {
  return apiClient<PublishedTest[]>("/tests/published", {
    userId,
  });
}

export function publishTest(
  testId: number,
  userId: number
): Promise<PublishTestResult> {
  return apiClient<PublishTestResult>(
    `/tests/${testId}/publish`,
    {
      method: "POST",
      userId,
    }
  );
}

export function deleteTest(
  testId: number,
  userId: number
): Promise<{ message: string }> {
  return apiClient<{ message: string }>(
    `/tests/${testId}`,
    {
      method: "DELETE",
      userId,
    }
  );
}