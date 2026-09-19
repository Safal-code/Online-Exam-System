export interface ExamOption {
  optionId: number;
  optionText: string;
  optionOrder: number;
}

export interface ExamQuestion {
  questionId: number;
  questionText: string;
  questionOrder: number;
  options: ExamOption[];
}

export interface StartExamResult {
  testAttemptId: number;
  testId: number;
  testName: string;
  durationMinutes: number;
  startedAt: string;
  expiresAt: string;
  totalQuestions: number;
  questions: ExamQuestion[];
}

export interface SubmitAnswer {
  questionId: number;
  selectedOptionId: number | null;
}

export interface SubmitExamRequest {
  answers: SubmitAnswer[];
}