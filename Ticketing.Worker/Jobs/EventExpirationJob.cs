using MediatR;
using Ticketing.Application.Features.Events.Commands.FinishExpiredEvents;
using Ticketing.Application.Services;

namespace Ticketing.Worker.Jobs;

public class EventExpirationJob(IMediator mediator, ILogService logService) : IWorkerJob
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogService _logService = logService;

    public string Name => "EventExpirationJob";

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var finishedCount = await _mediator.Send(new FinishExpiredEventsCommand(), cancellationToken);
            logService.Info($"[Worker] {finishedCount} expired events marked as Finished.");
        }
        catch (Exception ex)
        {
            logService.Info($"[Worker] Error occurred while executing {Name}: {ex.Message}");
        }
    }
}
