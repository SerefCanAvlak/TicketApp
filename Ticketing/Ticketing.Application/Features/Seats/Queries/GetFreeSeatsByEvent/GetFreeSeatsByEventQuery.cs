using MediatR;
using Ticketing.Application.Features.Seats.Dtos;

namespace Ticketing.Application.Features.Seats.Queries.GetFreeSeatsByEvent;

public sealed record GetFreeSeatsByEventQuery(Guid EventId) : IRequest<List<SeatDto>>;
