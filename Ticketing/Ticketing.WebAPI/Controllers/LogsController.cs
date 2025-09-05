using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Features.Log.Dtos;
using Ticketing.Application.Features.LogRead.Queries.GetLatestLogs;
using Ticketing.WebAPI.Abstractions;

namespace Ticketing.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LogsController : ApiController
    {
        public LogsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<LogDto>>> GetLatest(int? take = 5)
        {
            var logs = await _mediator.Send(new GetLatestLogsQuery(take ?? int.MaxValue));
            return Ok(logs);
        }
    }
}
