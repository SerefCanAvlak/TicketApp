using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Features.Users.Dtos;
using Ticketing.Domain.Entities;

internal sealed class CreateUserCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.Users
            .FirstOrDefaultAsync(u =>
                u.Email == request.Email ||
                u.UserName == request.UserName,
                cancellationToken);

        if (existingUser is not null)
            throw new Exception("Bu Email veya Kullanıcı adı zaten kullanılıyor.");

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            EmailConfirmed = true,
            IsAdmin = request.IsAdmin,
            IsDeleted = false
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Kullanıcı oluşturulamadı: {errors}");
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