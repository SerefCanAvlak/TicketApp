using GenericRepository;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Interfaces;
using Ticketing.Infrastructure.Context;

namespace Ticketing.Infrastructure.Repositories;

internal sealed class TicketRepository : Repository<Ticket, ApplicationDbContext>, ITicketRepository
{
    public TicketRepository(ApplicationDbContext context) : base(context)
    {
    }
}

