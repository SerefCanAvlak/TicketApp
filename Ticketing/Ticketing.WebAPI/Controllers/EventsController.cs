using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Features.Events.Commands.CreateEvent;
using Ticketing.Application.Features.Events.Commands.DeleteEvent;
using Ticketing.Application.Features.Events.Commands.UpdateEvent;
using Ticketing.Application.Features.Events.Dtos;
using Ticketing.Application.Features.Events.Queries.GetEvents;
using Ticketing.Application.Features.Events.Queries.GetUpcomingEvents;
using Ticketing.WebAPI.Abstractions;

namespace Ticketing.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EventsController : ApiController
    {
        public EventsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventCommand command, CancellationToken cancellationToken)
        {
            var createdEvent = await _mediator.Send(command, cancellationToken);
            return Ok(createdEvent);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEventsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EventDto>> UpdateEvent(Guid id, [FromBody] UpdateEventCommand command)
        {
            var commandWithId = command with { Id = id };
            var updatedEvent = await _mediator.Send(commandWithId);
            return Ok(updatedEvent);
        }



        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            await _mediator.Send(new DeleteEventByIdCommand(id));
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetUpcomingEvents(CancellationToken cancellationToken)
        {
            var events = await _mediator.Send(new GetUpcomingEventsQuery(), cancellationToken);
            return Ok(events);
        }

    }
}
