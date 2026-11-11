using RisVacationPlanning.Backend.Models;
using RisVacationPlanning.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC controllers are registered now; the tickets controller will receive
// its actions in the next laboratory work.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the service through DI. Do not instantiate it with `new` in a controller.
builder.Services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();

var app = builder.Build();

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
