using OnlineExamApi.DTOs.Auth;
using OnlineExamApi.Services;

namespace OnlineExamApi.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/login", async (LoginRequest request,AuthService authService) =>
        {
            var result = await authService.Login(request);

            if (result == null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(result);
        });

        app.MapPost( "/api/auth/register",async (RegisterRequest request, AuthService authService) =>
        {
         var result = await authService.Register(request);
         
         if (!result)
          {
             return Results.Conflict(
                 new
                 {
                     message = "Username already exists."
                 }
             );
          }
         
         return Results.Ok(
             new
             {
                 message = "Registration successful."

             }
         );
        });
    }
}
