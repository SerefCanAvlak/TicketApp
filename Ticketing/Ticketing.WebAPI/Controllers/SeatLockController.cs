using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Features.SeatLock.Commands.LockSeat;
using Ticketing.Application.Features.SeatLock.Queries;
using Ticketing.WebAPI.Abstractions;

namespace Ticketing.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SeatLockController : ApiController
{
    public SeatLockController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> LockSeat([FromBody] LockSeatCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result)
            return BadRequest("Seat is already locked or unavailable.");

        return Ok(result);
    }

    [HttpGet("{eventId:guid}")]
    public async Task<IActionResult> GetSeatLocks(Guid eventId)
    {
        var locks = await _mediator.Send(new GetSeatLocksQuery(eventId));
        return Ok(locks);
    }
}
