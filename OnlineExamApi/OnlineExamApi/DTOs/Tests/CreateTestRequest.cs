namespace OnlineExamApi.DTOs.Tests;

public class CreateTestRequest
{
    public string Name { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string? Description { get; set; }

    public int DurationMinutes { get; set; }

    public int PassingPercentage { get; set; } = 50; //use 50 if admin doesnt provide pass %
}
