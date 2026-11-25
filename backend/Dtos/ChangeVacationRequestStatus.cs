using System.ComponentModel.DataAnnotations;

namespace RisVacationPlanning.Backend.Dtos;

public sealed class ChangeVacationRequestStatus
{
    [Required]
    public string Status { get; init; } = string.Empty;
}
