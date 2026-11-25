using System.ComponentModel.DataAnnotations;

namespace RisVacationPlanning.Backend.Dtos;

public sealed class AssignVacationRequest
{
    [Range(1, int.MaxValue)]
    public int AssigneeUserId { get; init; }
}
