using MediatR;
using Ticketing.Application.Features.Users.Dtos;

namespace Ticketing.Application.Features.Users.Queries.GetAllUsersQuery;

public sealed record GetAllUsersQuery : IRequest<List<UserDto>> ;
