using Microsoft.AspNetCore.Identity;
using Ticketing.Domain.Entities;

namespace Ticketing.WebAPI.Middlewares
{
    public static class ExtensionsMiddleware
    {
        public static async Task CreateFirstUserAsync(WebApplication app)
        {
            using var scoped = app.Services.CreateScope();
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any(p => p.UserName == "admin"))
            {
                var user = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Şeref Can",
                    LastName = "Avlak",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "Admin123456");
            }
        }
    }
}
