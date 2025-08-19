using GenericRepository;
using MediatR;
using Ticketing.Application.Features.Ticket.Dtos;

namespace Ticketing.Application.Features.Ticket.Queries.GetAllTickets;

public sealed record GetAllTicketsQuery() : IRequest<List<TicketDto>>;
