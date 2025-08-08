using Ticketing.Application.Features.Auth.Login;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Services
{
    public interface IJwtProvider
    {
        Task<LoginCommandResponse> CreateToken(AppUser user);
    }
}
