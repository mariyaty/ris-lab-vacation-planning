using RisVacationPlanning.Backend.Models;

namespace RisVacationPlanning.Backend.Services;

public sealed class VacationRequestService
{
    private readonly List<VacationRequest> _items = [];
    private int _nextId = 1;

    public IReadOnlyList<VacationRequest> GetAll() =>
        _items.OrderByDescending(x => x.Id).ToList();

    public VacationRequest? GetById(int id) =>
        _items.FirstOrDefault(x => x.Id == id);

    public VacationRequest Create(string title, int siteId, string description)
    {
        var item = new VacationRequest(
            Id: _nextId,
            Number: $"VR-{_nextId:00000}",
            Title: title.Trim(),
            SiteId: siteId,
            Description: description.Trim(),
            Status: "New",
            CreatedByUserId: 5,
            AssigneeUserId: null,
            CreatedAt: DateTime.UtcNow
        );

        _nextId++;
        _items.Add(item);
        return item;
    }

    public VacationRequest? Assign(int id, int assigneeUserId)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return null;

        var updated = _items[index] with { AssigneeUserId = assigneeUserId };
        _items[index] = updated;
        return updated;
    }

    public (VacationRequest? Item, string? Conflict) ChangeStatus(int id, string status)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return (null, null);

        if (status == "InProgress" && _items[index].AssigneeUserId is null)
            return (_items[index], "Нельзя перевести заявку в InProgress без назначенного исполнителя.");

        var updated = _items[index] with { Status = status };
        _items[index] = updated;
        return (updated, null);
    }
}
