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
            e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new Exception("Event bulunamadı");

        if (!string.Equals(entity.State.Name, request.UpdateEventDto.State, StringComparison.OrdinalIgnoreCase))
        {
            entity.State = EventStateEnum.FromName(request.UpdateEventDto.State, true);
        }

        mapper.Map(request.UpdateEventDto, entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<EventDto>(entity);
    }
}