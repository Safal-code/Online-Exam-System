export interface CreateTestRequest {
  name: string;
  subject: string;
  description: string;
  durationMinutes: number;
  passingPercentage: number;
}

export interface Test {
  testId: number;
  name: string;
  subject: string;
  description: string | null;
  durationMinutes: number;
  passingPercentage: number;
  isPublished: boolean;
  createdBy: number;
  createdAt: string;
  publishedAt: string | null;
  usersAppeared: number;
}

export interface PublishedTest {
  testId: number;
  name: string;
  subject: string;
  description: string | null;
  durationMinutes: number;
  passingPercentage: number;
}

export interface PublishTestResult {
  testId: number;
  name: string;
  isPublished: boolean;
  publishedAt: string;
}