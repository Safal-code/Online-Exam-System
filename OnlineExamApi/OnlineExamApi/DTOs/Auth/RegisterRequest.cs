namespace OnlineExamApi.DTOs.Auth;

public class RegisterRequest
{
    public string UserName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;
}
