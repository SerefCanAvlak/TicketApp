using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Services;
using Ticketing.Domain.Enum;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Events.Commands.FinishExpiredEvents;

public sealed class FinishExpiredEventsCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork,
    ILogService logService) : IRequestHandler<FinishExpiredEventsCommand, int>
{
    public async Task<int> Handle(FinishExpiredEventsCommand request, CancellationToken cancellationToken)
    {
        var expiredEvents = await eventRepository
            .GetAll()
            .AsQueryable() 
            .Where(e => e.EventStart < DateTime.UtcNow && e.StateId != EventStateEnum.Finished.Value)
            .ToListAsync(cancellationToken);

        if (!expiredEvents.Any())
        {
            logService.Info("[Worker] No expired events to update.");
            return 0;
        }

        foreach (var ev in expiredEvents)
        {
            ev.State = EventStateEnum.Finished;
            eventRepository.Update(ev); // Ensure entity is tracked for update
            logService.Info($"[Worker] Event '{ev.Name}' marked as Finished.");
        }

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logService.Error("[Worker] Concurrency error occurred while updating events.", ex);
            throw;
        }
        catch (Exception ex)
        {
            logService.Error("[Worker] An error occurred while updating events.", ex);
            throw;
        }

        logService.Info($"[Worker] Total expired events finished: {expiredEvents.Count}");
        return expiredEvents.Count;
    }
}
