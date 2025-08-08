using Microsoft.AspNetCore.Identity;
using Ticketing.Domain.Entities;

namespace Ticketing.WebAPI.Middlewares
{
    public static class ExtensionsMiddleware
    {
        public static void CreateFirstUser(WebApplication app)
        {
            using (var scoped = app.Services.CreateScope())
            {
                var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                if (!userManager.Users.Any(p => p.UserName == "admin"))
                {
                    AppUser user = new()
                    {
                        UserName = "admin",
                        Email = "admin@admin.com",
                        FirstName = "Şeref Can",
                        LastName = "Avlak",
                        EmailConfirmed = true
                    };

                    userManager.CreateAsync(user, "Admin123456").Wait();
                }
            }
        }
    }
}
