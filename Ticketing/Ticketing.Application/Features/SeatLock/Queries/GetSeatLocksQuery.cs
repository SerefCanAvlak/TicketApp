using MediatR;

namespace Ticketing.Application.Features.SeatLock.Queries;

public sealed record GetSeatLocksQuery(Guid EventId) : IRequest<List<Ticketing.Domain.Entities.SeatLock>>;
