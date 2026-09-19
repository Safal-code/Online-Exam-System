namespace OnlineExamApi.DTOs.Tests;

public class PublishedTestResult
{
    public int TestId { get; set; }
    public string Name { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public int PassingPercentage { get; set; }
}
