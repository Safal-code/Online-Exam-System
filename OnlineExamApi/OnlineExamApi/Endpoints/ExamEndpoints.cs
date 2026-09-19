using OnlineExamApi.DTOs.Exam;
using OnlineExamApi.Services;

namespace OnlineExamApi.Endpoints;

public static class ExamEndpoints
{
    public static void MapExamEndpoints(this WebApplication app)
    {
        app.MapPost("/api/tests/{testId}/start", async (
            int testId,
            HttpRequest httpRequest,
            ExamService examService,
            RequestUserService requestUserService) =>
        {
            var currentUser =
                await requestUserService.GetCurrentUser(httpRequest);

            if (currentUser == null)
            {
                return Results.Unauthorized();
            }

            if (currentUser.Role != "User")
            {
                return Results.Forbid();
            }

            var result = await examService.StartExam(
                testId,
                currentUser.UserId);

            if (result == null)
            {
                return Results.NotFound(new
                {
                    message = "Published test not found."
                });
            }

            return Results.Ok(result);
        });

        app.MapPost("/api/exams/{attemptId}/submit", async (
            int attemptId,
            SubmitExamRequest request,
            HttpRequest httpRequest,
            ExamService examService,
            RequestUserService requestUserService) =>
        {
            var currentUser =
                await requestUserService.GetCurrentUser(httpRequest);

            if (currentUser == null)
            {
                return Results.Unauthorized();
            }

            if (currentUser.Role != "User")
            {
                return Results.Forbid();
            }

            try
            {
                var result = await examService.SubmitExam(
                    attemptId,
                    currentUser.UserId,
                    request);

                if (result == null)
                {
                    return Results.NotFound(new
                    {
                        message = "Exam attempt not found."
                    });
                }

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new
                {
                    message = ex.Message
                });
            }
        });

        app.MapGet("/api/exams/{attemptId}/result", async (
            int attemptId,
            HttpRequest httpRequest,
            ExamService examService,
            RequestUserService requestUserService) =>
        {
            var currentUser =
                await requestUserService.GetCurrentUser(httpRequest);

            if (currentUser == null)
            {
                return Results.Unauthorized();
            }

            if (currentUser.Role != "User")
            {
                return Results.Forbid();
            }

            var result = await examService.GetResult(
                attemptId,
                currentUser.UserId);

            if (result == null)
            {
                return Results.NotFound(new
                {
                    message = "Result not found."
                });
            }

            return Results.Ok(result);
        });
    }
}
