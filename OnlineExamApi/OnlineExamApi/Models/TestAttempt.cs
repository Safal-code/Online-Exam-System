using System;
using System.Collections.Generic;

namespace OnlineExamApi.Models;

public partial class TestAttempt
{
    public int TestAttemptId { get; set; }

    public int TestId { get; set; }

    public int UserId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int? Score { get; set; }

    public int TotalQuestions { get; set; }

    public bool? IsPassed { get; set; }

    public virtual Test Test { get; set; } = null!;

    public virtual ICollection<TestAnswer> TestAnswers { get; set; } = new List<TestAnswer>();

    public virtual User User { get; set; } = null!;
}
