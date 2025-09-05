using MediatR;
using Ticketing.Application.Features.Users.Dtos;

public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    bool IsAdmin) : IRequest<UserDto>;
