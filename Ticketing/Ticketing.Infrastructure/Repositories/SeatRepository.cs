using GenericRepository;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Interfaces;
using Ticketing.Infrastructure.Context;

namespace Ticketing.Infrastructure.Repositories;

internal sealed class SeatRepository : Repository<Seat, ApplicationDbContext>, ISeatRepository
{
    public SeatRepository(ApplicationDbContext context) : base(context)
    {
    }
}
