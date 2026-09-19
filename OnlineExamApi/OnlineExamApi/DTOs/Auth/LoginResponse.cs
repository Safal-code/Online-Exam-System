namespace OnlineExamApi.DTOs.Auth;

public class LoginResponse
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;
}
