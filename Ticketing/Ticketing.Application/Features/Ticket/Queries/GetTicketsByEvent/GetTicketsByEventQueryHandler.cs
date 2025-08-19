using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Ticket.Dtos;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Application.Features.Ticket.Queries.GetTicketsByEvent;

internal sealed class GetTicketsByEventQueryHandler(
    ITicketRepository ticketRepository,
    IMapper mapper) : IRequestHandler<GetTicketsByEventQuery, List<TicketDto>>
{
    public async Task<List<TicketDto>> Handle(GetTicketsByEventQuery request, CancellationToken cancellationToken)
    {
        var tickets = await ticketRepository.GetAll()
            .Where(t => t.EventId == request.EventId)
            .ToListAsync(cancellationToken);

        return tickets.Select(t => mapper.Map<TicketDto>(t)).ToList();
    }
}