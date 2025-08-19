namespace Ticketing.Application.Features.Events.Dtos
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime EventStart { get; set; }
        public Guid PhysicalSeatLayoutId { get; set; }
        public required string State { get; set; }
    }
}
