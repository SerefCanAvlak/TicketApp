using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Features.Ticket.Commands.CancelTicket;
using Ticketing.Application.Features.Ticket.Commands.CreateTicket;
using Ticketing.Application.Features.Ticket.Dtos;
using Ticketing.Application.Features.Ticket.Queries.GetAllTickets;
using Ticketing.Application.Features.Ticket.Queries.GetTicketsByEvent;

namespace Ticketing.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<TicketDto?>> CreateTicket([FromBody] CreateTicketCommand command)
        {
            var ticket = await _mediator.Send(command);

            if (ticket == null)
                return BadRequest("Seat is already taken or lock failed.");

            return Ok(ticket);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CancelTicketCommand(id), cancellationToken);

            if (result is null)
                return NotFound("Ticket not found or cannot be cancelled.");

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var tickets = await _mediator.Send(new GetAllTicketsQuery(), cancellationToken);
            return Ok(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> GetTicketsByEvent(Guid eventId)
        {
            var tickets = await _mediator.Send(new GetTicketsByEventQuery(eventId));
            return Ok(tickets);
        }
    }
}
