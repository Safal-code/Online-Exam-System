using Microsoft.EntityFrameworkCore;
using OnlineExamApi.Data;
using OnlineExamApi.DTOs.Questions;
using OnlineExamApi.Models;

namespace OnlineExamApi.Services;

public class QuestionService
{
    private readonly ExamDbContext _dbContext;

    public QuestionService(ExamDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<QuestionResult> Create(
        int testId,
        CreateQuestionRequest request)
    {
        var test = await _dbContext.Tests
            .FirstOrDefaultAsync(t => t.TestId == testId);

        if (test == null)
        {
            throw new Exception("Test not found.");
        }

        if (test.IsPublished)
        {
            throw new Exception(
                "Questions cannot be added after publishing.");
        }

        if (request.Options.Count < 2)
        {
            throw new Exception(
                "A question must have at least 2 options.");
        }

        var correctCount = request.Options.Count(o => o.IsCorrect);

        if (correctCount != 1)
        {
            throw new Exception(
                "A question must have exactly one correct option.");
        }

        var question = new Question
        {
            TestId = testId,
            QuestionText = request.QuestionText,
            QuestionOrder = request.QuestionOrder
        };

        foreach (var optionRequest in request.Options)
        {
            question.Options.Add(new Option
            {
                OptionText = optionRequest.OptionText,
                IsCorrect = optionRequest.IsCorrect,
                OptionOrder = optionRequest.OptionOrder
            });
        }

        _dbContext.Questions.Add(question);

        await _dbContext.SaveChangesAsync();

        return new QuestionResult
        {
            QuestionId = question.QuestionId,
            TestId = question.TestId,
            QuestionText = question.QuestionText,
            QuestionOrder = question.QuestionOrder,
            Options = question.Options
                .Select(o => new OptionResult
                {
                    OptionId = o.OptionId,
                    OptionText = o.OptionText,
                    OptionOrder = o.OptionOrder
                })
                .ToList()
        };
    }

    public async Task<List<QuestionResult>> GetByTest(int testId)
    {
        return await _dbContext.Questions
            .Where(q => q.TestId == testId)
            .OrderBy(q => q.QuestionOrder)
            .Select(q => new QuestionResult
            {
                QuestionId = q.QuestionId,
                TestId = q.TestId,
                QuestionText = q.QuestionText,
                QuestionOrder = q.QuestionOrder,

                Options = q.Options
                    .OrderBy(o => o.OptionOrder)
                    .Select(o => new OptionResult
                    {
                        OptionId = o.OptionId,
                        OptionText = o.OptionText,
                        OptionOrder = o.OptionOrder
                    })
                    .ToList()
            })
            .ToListAsync();
    }
}
