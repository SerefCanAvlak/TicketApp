using AutoMapper;
using GenericRepository;
using MediatR;
using Ticketing.Application.Features.Events.Dtos;
using Ticketing.Domain.Enum;
using Ticketing.Domain.Interfaces;


namespace Ticketing.Application.Features.Events.Commands.UpdateEvent;

internal sealed class UpdateEventCommandHandler(
    IEventRepository eventRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateEventCommand, EventDto>
{
    public async Task<EventDto> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var entity = await eventRepository.GetByExpressionWithTrackingAsync(
    e => e.Id == request.Id, cancellationToken) ?? throw new Exception("Event bulunamadı");
        if (!string.Equals(entity.State.Name, request.State, StringComparison.OrdinalIgnoreCase))
        {
            entity.State = EventStateEnum.FromName(request.State, true);
        }

        entity.Name = request.Name;
        entity.EventStart = request.EventStart;
        entity.PhysicalSeatLayoutId = request.PhysicalSeatLayoutId;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<EventDto>(entity);
    }
}