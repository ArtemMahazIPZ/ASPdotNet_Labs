using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lab05.Data
{
    public static class IdentitySeed
    {
        public static async Task EnsureSeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var idDb = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            await idDb.Database.MigrateAsync();

            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            
            foreach (var role in new[] { "Admin", "User" })
            {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@example.com";
            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userMgr.CreateAsync(admin, "Admin123!");
                await userMgr.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
