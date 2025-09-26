using MediatR;
using Ticketing.Application.Features.Ticket.Dtos;

namespace Ticketing.Application.Features.Ticket.Commands.CreateTicket;

public sealed record CreateTicketCommand(
    Guid EventId,
    Guid SeatId,
    Guid UserId,
    string OwnerName) : IRequest<TicketDto?>;
