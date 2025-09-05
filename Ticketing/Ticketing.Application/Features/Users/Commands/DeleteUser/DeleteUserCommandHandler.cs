using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Features.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            throw new Exception("Kullanıcı bulunamadı");

        // Soft delete
        user.IsDeleted = true;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Kullanıcı silinemedi: {errors}");
        }

        return Unit.Value;
    }
}