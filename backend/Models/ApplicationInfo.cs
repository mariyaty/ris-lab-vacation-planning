namespace RisVacationPlanning.Backend.Models;

public sealed record ApplicationInfo(
    string ApplicationName,
    string Environment,
    string Version
);
