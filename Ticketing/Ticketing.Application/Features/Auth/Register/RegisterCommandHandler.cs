using MediatR;
using Microsoft.AspNetCore.Identity;
using Ticketing.Application.Services;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Features.Auth.Register;

internal sealed class RegisterCommandHandler(
    UserManager<AppUser> userManager,
    ILogService logService) : IRequestHandler<RegisterCommand, Guid>
{
    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            EmailConfirmed = true,
            Email = request.Email,
            IsAdmin = false
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"User creation failed: {errors}");
        }

        logService.Info($"Kullanıcı kayıt oldu. UserId: {user.Id}, UserName: {user.UserName}");

        return user.Id;
    }
}
