using AutoMapper;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Ticket.Dtos;
using Ticketing.Domain.Enum;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Ticket.Commands.CancelTicket;

internal sealed class CancelTicketCommandHandler(
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CancelTicketCommand, TicketDto?>
{
    public async Task<TicketDto?> Handle(CancelTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository
            .GetAll()
            .FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken);

        if (ticket is null)
            return null;

        if (ticket.StateId != TicketStateEnum.Valid.Value)
            return null;

        ticket.State = TicketStateEnum.Cancelled;

        ticketRepository.Update(ticket);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<TicketDto>(ticket);
    }
}