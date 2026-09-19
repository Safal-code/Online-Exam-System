namespace OnlineExamApi.DTOs.Exam;

public class ExamResult
{
    public int TestAttemptId { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public bool IsPassed { get; set; }
}
