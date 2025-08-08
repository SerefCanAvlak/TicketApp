using MediatR;

namespace Ticketing.Application.Features.Auth.Login
{
    public sealed record LoginCommand(
        string EmailOrUserName,
        string Password) : IRequest<LoginCommandResponse>;
}
