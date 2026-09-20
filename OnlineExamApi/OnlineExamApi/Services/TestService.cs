using Microsoft.EntityFrameworkCore;
using OnlineExamApi.Data;
using OnlineExamApi.DTOs.Tests;
using OnlineExamApi.Models;

namespace OnlineExamApi.Services;

public class TestService
{
    private readonly ExamDbContext _dbContext;

    public TestService(ExamDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //create the test by admin
    public async Task<CreateTestResult> Create(
     CreateTestRequest request,
     int userId)
    {
        var test = new Test
        {
            Name = request.Name,
            Subject = request.Subject,
            Description = request.Description,
            DurationMinutes = request.DurationMinutes,
            PassingPercentage = request.PassingPercentage,
            IsPublished = false,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Tests.Add(test);

        await _dbContext.SaveChangesAsync();

        return new CreateTestResult
        {
            TestId = test.TestId,
            Name = test.Name,
            Subject = test.Subject,
            Description = test.Description,
            DurationMinutes = test.DurationMinutes,
            PassingPercentage = test.PassingPercentage,
            IsPublished = test.IsPublished,
            CreatedBy = test.CreatedBy,
            CreatedAt = test.CreatedAt,
            PublishedAt = test.PublishedAt
        };
    }

    //get all test created by admin
    public async Task<List<TestListResult>> GetAll()
    {
        return await _dbContext.Tests
            .Select(t => new TestListResult
            {
                TestId = t.TestId,
                Name = t.Name,
                Subject = t.Subject,
                Description = t.Description,
                DurationMinutes = t.DurationMinutes,
                PassingPercentage = t.PassingPercentage,
                IsPublished = t.IsPublished,
                CreatedBy = t.CreatedBy,
                CreatedAt = t.CreatedAt,
                PublishedAt = t.PublishedAt,
                UsersAppeared = t.TestAttempts
                    .Select(a => a.UserId)
                    .Distinct()
                    .Count()
            })
            .ToListAsync();
    }

    public async Task<PublishTestResult?> Publish(int testId)
    {
        var test = await _dbContext.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(t => t.TestId == testId);

        if (test == null)
        {
            return null;
        }

        if (test.IsPublished)
        {
            return null;
        }

        if (test.Questions.Count == 0)
        {
            throw new Exception("Test must have at least one question.");
        }

        foreach (var question in test.Questions)
        {
            if (question.Options.Count < 2)
            {
                throw new Exception(
                    "Every question must have at least 2 options.");
            }

            var correctOptions = question.Options
                .Count(o => o.IsCorrect);

            if (correctOptions != 1)
            {
                throw new Exception(
                    "Every question must have exactly one correct option.");
            }
        }

        test.IsPublished = true;
        test.PublishedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new PublishTestResult
        {
            TestId = test.TestId,
            Name = test.Name,
            IsPublished = test.IsPublished,
            PublishedAt = test.PublishedAt!.Value
        };
    }

    //only return published test for users in home page
    public async Task<List<PublishedTestResult>> GetPublished()
    {
        return await _dbContext.Tests
            .Where(t => t.IsPublished)
            .Select(t => new PublishedTestResult
            {
                TestId = t.TestId,
                Name = t.Name,
                Subject = t.Subject,
                Description = t.Description,
                DurationMinutes = t.DurationMinutes,
                PassingPercentage = t.PassingPercentage
            })
            .ToListAsync();
    }

    //delete function for the test
    public async Task<bool> Delete(int testId)
    {
        var test = await _dbContext.Tests
            .FirstOrDefaultAsync(t => t.TestId == testId);

        if (test == null)
        {

            return false;
        }

        _dbContext.Tests.Remove(test);

        await _dbContext.SaveChangesAsync();

        return true;
    }
}
