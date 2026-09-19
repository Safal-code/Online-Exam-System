using System;
using System.Collections.Generic;

namespace OnlineExamApi.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int TestId { get; set; }

    public string QuestionText { get; set; } = null!;

    public int QuestionOrder { get; set; }

    public virtual ICollection<Option> Options { get; set; } = new List<Option>();

    public virtual Test Test { get; set; } = null!;

    public virtual ICollection<TestAnswer> TestAnswers { get; set; } = new List<TestAnswer>();
}
