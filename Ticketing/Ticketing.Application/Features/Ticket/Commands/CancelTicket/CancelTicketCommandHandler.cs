using AutoMapper;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Ticket.Dtos;
using Ticketing.Application.Services;
using Ticketing.Domain.Enum;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Ticket.Commands.CancelTicket;

internal sealed class CancelTicketCommandHandler(
    ITicketRepository ticketRepository,
    ILogService logService,
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

        logService.Info($"Bilet iptal edildi. TicketId: {ticket.Id}, EventId: {ticket.EventId}, SeatId: {ticket.SeatId}, OwnerName: {ticket.OwnerName}");

        return mapper.Map<TicketDto>(ticket);
    }
}