using AutoMapper;
using GenericRepository;
using MediatR;
using Ticketing.Application.Features.Events.Dtos;
using Ticketing.Application.Services;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enum;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Events.Commands.CreateEvent;

internal sealed class CreateEventCommandHandler(
    IEventRepository eventRepository,
    IMapper mapper,
    ILogService logService,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateEventCommand, EventDto>
{
    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var entity = new Event
        {
            Name = request.Name,
            EventStart = request.EventStart,
            PhysicalSeatLayoutId = request.PhysicalSeatLayoutId,
            State = EventStateEnum.FromName(request.State, true)
        };

        await eventRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logService.Info($"Etkinlik oluşturuldu. EventId: {entity.Id}, EventName: {entity.Name}");

        return mapper.Map<EventDto>(entity);
    }
}
