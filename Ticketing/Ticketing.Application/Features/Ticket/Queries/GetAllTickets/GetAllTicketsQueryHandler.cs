using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Ticket.Dtos;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Ticket.Queries.GetAllTickets;

internal sealed class GetAllTicketsQueryHandler(
    ITicketRepository ticketRepository,
    IMapper mapper) : IRequestHandler<GetAllTicketsQuery, List<TicketDto>>
{
    public async Task<List<TicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await ticketRepository
            .GetAll()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<TicketDto>>(tickets);
    }
}
