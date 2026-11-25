using Microsoft.AspNetCore.Mvc;
using RisVacationPlanning.Backend.Dtos;
using RisVacationPlanning.Backend.Services;\nusing RisVacationPlanning.Backend.Models;

namespace RisVacationPlanning.Backend.Controllers;

[ApiController]
[Route("api/vacation-requests")]
public sealed class VacationRequestsController : ControllerBase
{
    private static readonly HashSet<string> AllowedStatuses =
        ["New", "InProgress", "Closed", "Cancelled"];

    private readonly VacationRequestService _service;
    private readonly ILogger<VacationRequestsController> _logger;

    public VacationRequestsController(
        VacationRequestService service,
        ILogger<VacationRequestsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<VacationRequestResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<VacationRequestResponse>> GetAll()
    {
        var result = _service.GetAll().Select(ToResponse).ToList();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(VacationRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<VacationRequestResponse> GetById(int id)
    {
        var item = _service.GetById(id);
        return item is null ? NotFound(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Заявка не найдена."
        }) : Ok(ToResponse(item));
    }

    [HttpPost]
    [ProducesResponseType(typeof(VacationRequestResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<VacationRequestResponse> Create([FromBody] CreateVacationRequest request)
    {
        try
        {
            var item = _service.Create(request.Title, request.SiteId, request.Description);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, ToResponse(item));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "CreateVacationRequest failed; siteId={SiteId}; error={ErrorType}",
                request.SiteId, ex.GetType().Name);

            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Не удалось создать заявку.",
                detail: "На сервере произошла непредвиденная ошибка. Повторите попытку позже.");
        }
    }

    [HttpPatch("{id:int}/assignee")]
    [ProducesResponseType(typeof(VacationRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<VacationRequestResponse> Assign(int id, [FromBody] AssignVacationRequest request)
    {
        var item = _service.Assign(id, request.AssigneeUserId);
        return item is null ? NotFound(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Заявка не найдена."
        }) : Ok(ToResponse(item));
    }

    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(VacationRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<VacationRequestResponse> ChangeStatus(
        int id, [FromBody] ChangeVacationRequestStatus request)
    {
        if (!AllowedStatuses.Contains(request.Status))
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Недопустимый статус.",
                Detail = "Допустимые значения: New, InProgress, Closed, Cancelled."
            });
        }

        var (item, conflict) = _service.ChangeStatus(id, request.Status);
        if (item is null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Заявка не найдена."
            });
        }

        if (conflict is not null)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Нарушено правило процесса.",
                Detail = conflict
            });
        }

        return Ok(ToResponse(item));
    }

    private static VacationRequestResponse ToResponse(VacationRequest item) =>
        new(item.Id, item.Number, item.Title, item.SiteId, item.Description,
            item.Status, item.CreatedByUserId, item.AssigneeUserId, item.CreatedAt);
}
