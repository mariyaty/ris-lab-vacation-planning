using RisVacationPlanning.Backend.Models;

namespace RisVacationPlanning.Backend.Services;

public sealed class ApplicationInfoService : IApplicationInfoService
{
    private readonly IHostEnvironment _environment;

    public ApplicationInfoService(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public ApplicationInfo GetInfo()
    {
        return new ApplicationInfo(
            ApplicationName: "ИИ-система планирования графика отпусков",
            Environment: _environment.EnvironmentName,
            Version: "ЛР7"
        );
    }
}
