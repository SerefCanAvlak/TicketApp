using GenericRepository;
using Ticketing.Domain.Entities;

namespace Ticketing.Domain.Interfaces;

public interface IEventRepository : IRepository<Event>
{
}
