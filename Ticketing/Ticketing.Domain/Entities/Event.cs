using Ticketing.Domain.Abstractions;
using Ticketing.Domain.Enum;

namespace Ticketing.Domain.Entities;

public sealed class Event : Entity
{
    public string Name { get; set; } = string.Empty;
    public DateTime EventStart { get; set; }
    public int StateId { get; set; }
    public EventStateEnum State
    {
        get => EventStateEnum.FromValue(StateId);
        set => StateId = value.Value;
    }
    public Guid PhysicalSeatLayoutId { get; set; }
}