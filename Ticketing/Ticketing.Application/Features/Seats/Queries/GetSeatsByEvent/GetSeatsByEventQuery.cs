using MediatR;
using Ticketing.Application.Features.Seats.Dtos;

namespace Ticketing.Application.Features.Seats.Queries.GetSeatsByEvent;

public sealed record GetSeatsByEventQuery(Guid EventId): IRequest<List<SeatDto>>;
