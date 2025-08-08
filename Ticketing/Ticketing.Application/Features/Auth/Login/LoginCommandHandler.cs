using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Services;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Features.Auth.Login
{
    internal sealed class LoginCommandHandler(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, LoginCommandResponse>
    {
        public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await userManager.Users
                .FirstOrDefaultAsync(p =>
                    p.UserName == request.EmailOrUserName ||
                    p.Email == request.EmailOrUserName,
                    cancellationToken);

            if (user is null)
                throw new Exception("Kullanıcı bulunamadı");

            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

            if (signInResult.IsLockedOut)
            {
                TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
                if (timeSpan is not null)
                    throw new Exception($"Kullanıcı bloke. {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika bekleyin.");
                else
                    throw new Exception("Kullanıcı bloke.");
            }

            if (signInResult.IsNotAllowed)
                throw new Exception("Mail adresiniz onaylı değil");

            if (!signInResult.Succeeded)
                throw new Exception("Şifreniz yanlış");

            return await jwtProvider.CreateToken(user);
        }
    }
}
