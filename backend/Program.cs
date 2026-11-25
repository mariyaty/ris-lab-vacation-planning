using RisVacationPlanning.Backend.Models;
using RisVacationPlanning.Backend.Services;\nusing Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
// ЛР8: хранилище заявок существует в памяти процесса.
builder.Services.AddSingleton<VacationRequestService>();

var app = builder.Build();

// Единая безопасная обработка неожиданных ошибок: подробности остаются на сервере.
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception)
    {
        var logger = context.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("GlobalExceptionHandler");

        logger.LogError("Unhandled server error; path={Path}", context.Request.Path);

        if (!context.Response.HasStarted)
        {
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Внутренняя ошибка сервера.",
                Detail = "Произошла непредвиденная ошибка. Повторите попытку позже."
            });
        }
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok("ok"))
    .WithName("Health")
    .WithTags("Health")
    .Produces<string>(StatusCodes.Status200OK);

app.MapGet("/info", (IApplicationInfoService infoService) =>
        Results.Ok(infoService.GetInfo()))
    .WithName("ApplicationInfo")
    .WithTags("Info")
    .Produces<ApplicationInfo>(StatusCodes.Status200OK);

app.MapControllers();
app.Run();
