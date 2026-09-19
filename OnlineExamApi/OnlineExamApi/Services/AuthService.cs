using Microsoft.EntityFrameworkCore;
using OnlineExamApi.Data;
using OnlineExamApi.DTOs.Auth;
using OnlineExamApi.Models;

namespace OnlineExamApi.Services;

public class AuthService
{
    private readonly ExamDbContext _dbContext;

    public AuthService(ExamDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LoginResponse?> Login(LoginRequest request)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u =>
                u.UserName == request.UserName);

        if (user == null)
        {
            return null;
        }

        if (user.PasswordHash != request.Password)
        {
            return null;
        }

        return new LoginResponse
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Name = user.Name,
            Role = user.Role
        };
    }

    public async Task<bool> Register(
    RegisterRequest request)
    {
        var existingUser =
            await _dbContext.Users
                .FirstOrDefaultAsync(
                    u => u.UserName == request.UserName
                );

        if (existingUser != null)
        {
            return false;
        }

        var user = new User
        {
            UserName = request.UserName,
            Name = request.Name,

            // Temporary plaintext password, 
            PasswordHash = request.Password,

            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        return true;
    }
}
