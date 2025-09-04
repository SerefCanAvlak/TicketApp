using MediatR;
using Microsoft.AspNetCore.Identity;
using Ticketing.Application.Features.Users.Dtos;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Features.Users.Queries.GetAllUsersQuery;

public sealed record GetAllUsersQuery : IRequest<List<UserDto>> ;


internal sealed class GetAllUsersQueryHandler(
    UserManager<AppUser> userManager) : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    public Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = userManager.Users
                .Where(u => !u.IsDeleted)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName ?? string.Empty,
                    Email = u.Email ?? string.Empty,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsAdmin = u.IsAdmin,
                    IsDeleted = u.IsDeleted
                })
                .ToList();

        return Task.FromResult(users);
    }
}