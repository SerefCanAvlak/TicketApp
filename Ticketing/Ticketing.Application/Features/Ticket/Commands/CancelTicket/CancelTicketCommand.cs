using MediatR;
using Ticketing.Application.Features.Ticket.Dtos;

namespace Ticketing.Application.Features.Ticket.Commands.CancelTicket;

public sealed record CancelTicketCommand(
    Guid TicketId) : IRequest<TicketDto?>;
