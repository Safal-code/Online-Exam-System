using Microsoft.EntityFrameworkCore;
using OnlineExamApi.Data;
using OnlineExamApi.Endpoints;
using OnlineExamApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ExamDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("OnlineExamConnection")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<RequestUserService>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<ExamService>();

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

app.UseHttpsRedirection();
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
