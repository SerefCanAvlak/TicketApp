using MediatR;
using Ticketing.Application.Features.Users.Dtos;

namespace Ticketing.Application.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? Password,
    bool IsAdmin) : IRequest<UserDto>;
