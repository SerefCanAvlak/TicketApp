using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Users.Dtos;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Features.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<UpdateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            throw new Exception("Kullanıcı bulunamadı");

        // Email veya UserName başka bir kullanıcıda var mı kontrol
        var conflictUser = await userManager.Users
            .FirstOrDefaultAsync(u =>
                (u.Email == request.Email || u.UserName == request.UserName) && u.Id != request.Id,
                cancellationToken);

        if (conflictUser is not null)
            throw new Exception("Email veya kullanıcı adı başka bir kullanıcıda kullanılıyor");

        // Alanları güncelle
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.UserName = request.UserName;
        user.Email = request.Email;
        user.IsAdmin = request.IsAdmin;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Kullanıcı güncellenemedi: {errors}");
        }

        // Şifre değişimi varsa
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var pwdResult = await userManager.ResetPasswordAsync(user, token, request.Password);
            if (!pwdResult.Succeeded)
            {
                var pwdErrors = string.Join(", ", pwdResult.Errors.Select(e => e.Description));
                throw new Exception($"Şifre güncellenemedi: {pwdErrors}");
            }
        }

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            IsAdmin = user.IsAdmin,
            IsDeleted = user.IsDeleted
        };
    }
}