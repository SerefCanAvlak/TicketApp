using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Features.Seats.Dtos;
using Ticketing.Application.Features.Seats.Queries.GetFreeSeatsByEvent;
using Ticketing.Application.Features.Seats.Queries.GetSeatsByEvent;
using Ticketing.WebAPI.Abstractions;

namespace Ticketing.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeatsController : ApiController
    {
        public SeatsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<SeatDto>>> GetSeatsByEvent(Guid eventId)
        {
            var seats = await _mediator.Send(new GetSeatsByEventQuery(eventId));

            if (seats == null || !seats.Any())
                return NotFound("Event bulunamadı veya koltuk yok.");

            return Ok(seats);
        }

        [HttpGet]
        public async Task<IActionResult> GetFreeSeatsByEvent(Guid eventId)
        {
            var seats = await _mediator.Send(new GetFreeSeatsByEventQuery(eventId));
            return Ok(seats);
        }

    }
}
