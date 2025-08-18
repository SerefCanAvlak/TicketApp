using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Features.Ticket.Commands.CreateTicket;
using Ticketing.Application.Features.Ticket.Dtos;

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
    }
}
