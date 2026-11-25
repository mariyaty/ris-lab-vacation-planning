namespace RisVacationPlanning.Backend.Models;

public sealed record VacationRequest(
    int Id,
    string Number,
    string Title,
    int SiteId,
    string Description,
    string Status,
    int CreatedByUserId,
    int? AssigneeUserId,
    DateTime CreatedAt
);
