namespace OnlineExamApi.DTOs.Tests;

public class CreateTestResult
{
    public int TestId { get; set; }

    public string Name { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string? Description { get; set; }

    public int DurationMinutes { get; set; }

    public int PassingPercentage { get; set; }

    public bool IsPublished { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PublishedAt { get; set; }
}
