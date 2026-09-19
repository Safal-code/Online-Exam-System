namespace OnlineExamApi.DTOs.Exam;

public class SubmitExamRequest
{
    public List<SubmitAnswerRequest> Answers { get; set; } = new();
}

public class SubmitAnswerRequest
{
    public int QuestionId { get; set; }
    public int? SelectedOptionId { get; set; }
}
