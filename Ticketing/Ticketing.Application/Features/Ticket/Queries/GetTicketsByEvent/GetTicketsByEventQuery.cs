using MediatR;
using Ticketing.Application.Features.Ticket.Dtos;

namespace Ticketing.Application.Features.Ticket.Queries.GetTicketsByEvent;

public sealed record GetTicketsByEventQuery(Guid EventId) : IRequest<List<TicketDto>>;
