namespace OnlineExamApi.DTOs.Exam;

public class StartExamResult
{
    public int TestAttemptId { get; set; }
    public int TestId { get; set; }
    public string TestName { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int TotalQuestions { get; set; }
    public List<ExamQuestionResult> Questions { get; set; } = new();
}

public class ExamQuestionResult
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = null!;
    public int QuestionOrder { get; set; }
    public List<ExamOptionResult> Options { get; set; } = new();
}

public class ExamOptionResult
{
    public int OptionId { get; set; }
    public string OptionText { get; set; } = null!;
    public int OptionOrder { get; set; }
}

//sent by backend
