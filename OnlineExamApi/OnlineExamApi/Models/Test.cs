using System;
using System.Collections.Generic;

namespace OnlineExamApi.Models;

public partial class Test
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

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<TestAttempt> TestAttempts { get; set; } = new List<TestAttempt>();
}
