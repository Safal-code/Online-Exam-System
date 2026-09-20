using Microsoft.EntityFrameworkCore;
using OnlineExamApi.Data;
using OnlineExamApi.Models;

namespace OnlineExamApi.Services;

public class CurrentUserService
{
    private readonly ExamDbContext _dbContext;

    public CurrentUserService(ExamDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUser(int userId)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }
}

//finds a user using their id 
//coming userid from requestUserService 
