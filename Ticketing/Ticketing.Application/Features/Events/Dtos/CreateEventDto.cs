namespace Ticketing.Application.Features.Events.Dtos;

public class CreateEventDto
{
    public required string Name { get; set; }
    public DateOnly EventStart { get; set; }
    public Guid PhysicalSeatLayoutId { get; set; }
    public required string State { get; set; }
}
