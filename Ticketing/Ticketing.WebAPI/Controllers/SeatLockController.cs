using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Features.SeatLock.Commands.LockSeat;
using Ticketing.Application.Features.SeatLock.Queries;

namespace Ticketing.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SeatLockController : ControllerBase
{
    private readonly IMediator _mediator;

    public SeatLockController(IMediator mediator)
    {
        _mediator = mediator;
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
