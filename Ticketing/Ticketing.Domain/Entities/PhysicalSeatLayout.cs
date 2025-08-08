using Ticketing.Domain.Abstractions;

namespace Ticketing.Domain.Entities;

public sealed class PhysicalSeatLayout : Entity
{
    public string Name { get; set; } = string.Empty;

}
