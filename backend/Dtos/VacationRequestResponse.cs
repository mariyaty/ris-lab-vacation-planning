namespace RisVacationPlanning.Backend.Dtos;

public sealed record VacationRequestResponse(
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
