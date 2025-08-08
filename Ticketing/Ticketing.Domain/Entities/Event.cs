using Ticketing.Domain.Abstractions;
using Ticketing.Domain.Enum;

namespace Ticketing.Domain.Entities;

public sealed class Event : Entity
{
    public string Name { get; set; } = string.Empty;
    public DateOnly EventStart { get; set; }
    public EventStateEnum State { get; set; } = EventStateEnum.Available;
    public Guid PhysicalSeatLayoutId { get; set; }
}
