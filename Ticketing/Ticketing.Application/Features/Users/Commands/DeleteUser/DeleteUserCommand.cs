using MediatR;
using Ticketing.Application.Features.Users.Dtos;

namespace Ticketing.Application.Features.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : IRequest<Unit>;
