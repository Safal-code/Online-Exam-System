namespace OnlineExamApi.DTOs.Questions;

public class CreateQuestionRequest
{
    public string QuestionText { get; set; } = null!;
    public int QuestionOrder { get; set; }
    public List<CreateOptionRequest> Options { get; set; } = new();
}

public class CreateOptionRequest
{
    public string OptionText { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public int OptionOrder { get; set; }
}
