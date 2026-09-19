using System;
using System.Collections.Generic;

namespace OnlineExamApi.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    //means test result belong to this user
    public virtual ICollection<TestAttempt> TestAttempts { get; set; } = new List<TestAttempt>();

    //means test created by this user
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
