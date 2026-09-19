using System;
using System.Collections.Generic;

namespace OnlineExamApi.Models;

public partial class TestAnswer
{
    public int TestAnswerId { get; set; }

    public int TestAttemptId { get; set; }

    public int QuestionId { get; set; }

    public int? SelectedOptionId { get; set; }

    public bool IsCorrect { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual Option? SelectedOption { get; set; }

    public virtual TestAttempt TestAttempt { get; set; } = null!;
}
