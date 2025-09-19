using GenericRepository;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Interfaces;
using Ticketing.Infrastructure.Context;

namespace Ticketing.Infrastructure.Repositories;

public sealed class EventRepository : Repository<Event, ApplicationDbContext>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }
}
