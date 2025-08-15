using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Features.Events.Commands.CreateEvent;
using Ticketing.Application.Features.Events.Commands.DeleteEvent;
using Ticketing.Application.Features.Events.Commands.UpdateEvent;
using Ticketing.Application.Features.Events.Dtos;
using Ticketing.Application.Features.Events.Queries;

namespace Ticketing.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> Create([FromBody] CreateEventDto createEventDto)
        {
            var command = new CreateEventCommand(createEventDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEventsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] UpdateEventDto updateEventDto, CancellationToken cancellationToken)
        {
                var command = new UpdateEventCommand(id, updateEventDto);
                var updatedEvent = await _mediator.Send(command, cancellationToken);
                return Ok(updatedEvent);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            await _mediator.Send(new DeleteEventByIdCommand(id));
            return NoContent();
        }
    }
}
