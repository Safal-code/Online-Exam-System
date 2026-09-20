using Microsoft.EntityFrameworkCore;
using OnlineExamApi.Data;
using OnlineExamApi.Endpoints;
using OnlineExamApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ExamDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("OnlineExamConnection")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TestService>(); //create, get, publish, delete test
builder.Services.AddScoped<CurrentUserService>(); //used to find user by their id
builder.Services.AddScoped<RequestUserService>(); //used to read X-User-Id from http request
builder.Services.AddScoped<QuestionService>(); //create and retrieve question
builder.Services.AddScoped<ExamService>(); //start , submit exam , get result //AddScopte for 1 obj per request

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection(); //redirects http req to https
app.UseCors("Frontend");

//app.MapGet("/api/test-db", async (ExamDbContext db) =>
//{
//    var count = await db.Users.CountAsync();

//    return Results.Ok(new
//    {
//        message = "Database connection successful",
//        userCount = count
//    });
//});

app.MapAuthEndpoints();
app.MapTestEndpoints();
app.MapQuestionEndpoints();
app.MapExamEndpoints();

app.Run();








//when app starts it is start point of app Program.cs says:
//"Use this database, use these services, allow this frontend, create these API endpoints, and now start the application."
