using OnlineExamApi.DTOs.Tests;
using OnlineExamApi.Services;

namespace OnlineExamApi.Endpoints;

public static class TestEndpoints
{
    public static void MapTestEndpoints(this WebApplication app)
    {
        app.MapPost("/api/tests", async (
            CreateTestRequest request,
            HttpRequest httpRequest,   
            TestService testService,
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

            var result = await testService.Create(request, currentUser.UserId);

            return Results.Ok(result);
        });

        app.MapGet("/api/tests", async (
            HttpRequest httpRequest,
            TestService testService,
            RequestUserService requestUserService) =>
        {
            var currentUser = await requestUserService.GetCurrentUser(httpRequest);

            if (currentUser == null)
            {
                return Results.Unauthorized();
            }

            if (currentUser.Role != "Admin")
            {
                return Results.StatusCode(403);
            }

            var result = await testService.GetAll();

            return Results.Ok(result);
        });

        app.MapPost("/api/tests/{testId}/publish", async (
            int testId,
            HttpRequest httpRequest,
            TestService testService,
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
                var result = await testService.Publish(testId);

                if (result == null)
                {
                    return Results.NotFound(new
                    {
                        message = "Test not found or already published."
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

        app.MapGet("/api/tests/published", async (
            HttpRequest httpRequest,
            TestService testService,
            RequestUserService requestUserService) =>
        {
            var currentUser = await requestUserService.GetCurrentUser(httpRequest);

            if (currentUser == null)
            {
                return Results.Unauthorized();
            }

            if (currentUser.Role != "User")
            {
                return Results.Forbid();
            }

            var result = await testService.GetPublished();

            return Results.Ok(result);
        });

        app.MapDelete("/api/tests/{testId:int}",async (
        int testId,
        HttpRequest httpRequest,
        TestService testService,
        RequestUserService requestUserService) =>
        {
        var currentUser =
            await requestUserService
                .GetCurrentUser(httpRequest);

        if (currentUser == null)
        {
            return Results.Unauthorized();
        }

        if (currentUser.Role != "Admin")
        {
            return Results.Forbid();
        }

        var deleted =
            await testService.Delete(testId);

        if (!deleted)
        {
            return Results.NotFound(
                new
                {
                    message = "Test not found."
                });
        }

        return Results.Ok(
            new
            {
                message = "Test deleted successfully."
            });
    });

    }
}
