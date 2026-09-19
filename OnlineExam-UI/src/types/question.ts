export interface CreateOptionRequest {
  optionText: string;
  isCorrect: boolean;
  optionOrder: number;
}

export interface CreateQuestionRequest {
  questionText: string;
  questionOrder: number;
  options: CreateOptionRequest[];
}

export interface Option {
  optionId: number;
  optionText: string;
  optionOrder: number;
}

export interface Question {
  questionId: number;
  testId: number;
  questionText: string;
  questionOrder: number;
  options: Option[];
}