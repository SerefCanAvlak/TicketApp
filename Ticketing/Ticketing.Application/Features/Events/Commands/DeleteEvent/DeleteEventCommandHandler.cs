using AutoMapper;
using GenericRepository;
using MediatR;
using Ticketing.Application.Features.Events.Commands.DeleteEvent;
using Ticketing.Application.Features.Events.Dtos;
using Ticketing.Application.Services;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Events.Commands.DeleteEvet;

internal sealed class DeleteEventCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork,
    ILogService logService,
    IMapper mapper) : IRequestHandler<DeleteEventByIdCommand, EventDto>
{
    public async Task<EventDto> Handle(DeleteEventByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await eventRepository.GetByExpressionAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new Exception("Event bulunamadı");

        var dto = mapper.Map<EventDto>(entity);

        await eventRepository.DeleteByExpressionAsync(e => e.Id == request.Id, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logService.Info($"Etkinlik silindi. EventId: {entity.Id}, EventName: {entity.Name}");

        return dto;
    }
}