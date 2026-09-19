namespace OnlineExamApi.DTOs.Questions;

public class QuestionResult
{
    public int QuestionId { get; set; }
    public int TestId { get; set; }
    public string QuestionText { get; set; } = null!;
    public int QuestionOrder { get; set; }
    public List<OptionResult> Options { get; set; } = new();
}

public class OptionResult
{
    public int OptionId { get; set; }
    public string OptionText { get; set; } = null!;
    public int OptionOrder { get; set; }
}
