using Microsoft.EntityFrameworkCore;
using OnlineExamApi.Data;
using OnlineExamApi.DTOs.Exam;
using OnlineExamApi.Models;

namespace OnlineExamApi.Services;

public class ExamService
{
    private readonly ExamDbContext _dbContext;

    public ExamService(ExamDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    //start exam and handle duration of it 
    public async Task<StartExamResult?> StartExam(int testId,int userId)
    {
        var test = await _dbContext.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Options) //have to use it to go level deeper
            .FirstOrDefaultAsync(t => t.TestId == testId);

        if (test == null || !test.IsPublished)
        {
            return null;
        }

        var now = DateTime.UtcNow;

        var attempt = new TestAttempt
        {
            TestId = testId,
            UserId = userId,
            StartedAt = now,
            ExpiresAt = now.AddMinutes(test.DurationMinutes),
            TotalQuestions = test.Questions.Count
        };

        _dbContext.TestAttempts.Add(attempt);

        await _dbContext.SaveChangesAsync();

        return new StartExamResult
        {
            TestAttemptId = attempt.TestAttemptId,
            TestId = test.TestId,
            TestName = test.Name,
            DurationMinutes = test.DurationMinutes,
            StartedAt = attempt.StartedAt,
            ExpiresAt = attempt.ExpiresAt,
            TotalQuestions = test.Questions.Count,

            Questions = test.Questions
                .OrderBy(q => q.QuestionOrder)  //q is test.Questions
                .Select(q => new ExamQuestionResult
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    QuestionOrder = q.QuestionOrder,

                    Options = q.Options
                        .OrderBy(o => o.OptionOrder)
                        .Select(o => new ExamOptionResult
                        {
                            OptionId = o.OptionId,
                            OptionText = o.OptionText,
                            OptionOrder = o.OptionOrder
                        })
                        .ToList()
                })
                .ToList()
        };
    }

    public async Task<ExamResult?> SubmitExam(
    int attemptId,
    int userId,
    SubmitExamRequest request)
    {
        var attempt = await _dbContext.TestAttempts
            .Include(a => a.Test)
            .ThenInclude(t => t.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(a =>
                a.TestAttemptId == attemptId &&
                a.UserId == userId);

        if (attempt == null)
        {
            return null;
        }

        if (attempt.CompletedAt != null)
        {
            throw new Exception("Exam has already been submitted.");
        }

        if (DateTime.UtcNow > attempt.ExpiresAt)
        {
            throw new Exception("Exam time has expired.");
        }

        var correctAnswers = 0;

        //loops finds 1 question at a time and check answer and save it in test answers
        foreach (var answer in request.Answers)
        {
            var question = attempt.Test.Questions
                .FirstOrDefault(q => q.QuestionId == answer.QuestionId);

            if (question == null)
            {
                throw new Exception("Invalid question.");
            }

            Option? selectedOption = null; //default selected option is null

            if (answer.SelectedOptionId.HasValue)//if user selected something
            {
                selectedOption = question.Options
                    .FirstOrDefault(o =>
                        o.OptionId == answer.SelectedOptionId.Value);

                if (selectedOption == null)
                {
                    throw new Exception(
                        "Selected option does not belong to the question.");
                }
            }

            var isCorrect =
                selectedOption != null &&
                selectedOption.IsCorrect;

            if (isCorrect)
            {
                correctAnswers++;
            }

            _dbContext.TestAnswers.Add(new TestAnswer
            {
                TestAttemptId = attempt.TestAttemptId,
                QuestionId = question.QuestionId,
                SelectedOptionId = answer.SelectedOptionId,
                IsCorrect = isCorrect
            });
        }

        var score = attempt.TotalQuestions == 0
            ? 0
            : (correctAnswers * 100) / attempt.TotalQuestions;

        var isPassed = score >= attempt.Test.PassingPercentage;

        attempt.Score = score;
        attempt.IsPassed = isPassed;
        attempt.CompletedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new ExamResult
        {
            TestAttemptId = attempt.TestAttemptId,
            Score = score,
            TotalQuestions = attempt.TotalQuestions,
            IsPassed = isPassed
        };
    }

    public async Task<ExamResult?> GetResult(
    int attemptId,
    int userId)
    {
        var attempt = await _dbContext.TestAttempts
            .FirstOrDefaultAsync(a =>
                a.TestAttemptId == attemptId &&
                a.UserId == userId);

        if (attempt == null || attempt.Score == null)
        {
            return null;
        }

        return new ExamResult
        {
            TestAttemptId = attempt.TestAttemptId,
            Score = attempt.Score.Value,
            TotalQuestions = attempt.TotalQuestions,
            IsPassed = attempt.IsPassed ?? false
        };
    }


}


//it controls startind exam
//submitting exam
//calculating score
//retrieving result