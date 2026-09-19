using OnlineExamApi.DTOs.Questions;
using OnlineExamApi.Services;

namespace OnlineExamApi.Endpoints;

public static class QuestionEndpoints
{
    public static void MapQuestionEndpoints(this WebApplication app)
    {
        app.MapPost("/api/tests/{testId}/questions", async (
            int testId,
            CreateQuestionRequest request,
            HttpRequest httpRequest,
            QuestionService questionService,
            RequestUserService requestUserService) =>
        {
            var currentUser =
                await requestUserService.GetCurrentUser(httpRequest);

            if (currentUser == null)
            {
                return Results.Unauthorized();
            }

            if (currentUser.Role != "Admin")
            {
                return Results.Forbid();
            }

            try
            {
                var result = await questionService.Create(
                    testId,
                    request);

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

        app.MapGet("/api/tests/{testId}/questions", async (
            int testId,
            HttpRequest httpRequest,
            QuestionService questionService,
            RequestUserService requestUserService) =>
        {
            var currentUser =
                await requestUserService.GetCurrentUser(httpRequest);

            if (currentUser == null)
            {
                return Results.Unauthorized();
            }

            if (currentUser.Role != "Admin")
            {
                return Results.Forbid();
            }

            var result = await questionService.GetByTest(testId);

            return Results.Ok(result);
        });
    }
}
