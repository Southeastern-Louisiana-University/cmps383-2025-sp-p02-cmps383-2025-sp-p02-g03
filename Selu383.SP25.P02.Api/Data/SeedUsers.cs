using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Selu383.SP25.P02.Api.Features.Identity;


namespace Selu383.SP25.P02.Api.Data
{
    public static class SeedUsers
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            
            await context.Database.MigrateAsync();

            
            string[] roleNames = { "User", "Admin" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new Role { Name = roleName });
                }
            }

            
            await CreateUserIfNotExists(userManager, "bob", "Password123!", "User");
            await CreateUserIfNotExists(userManager, "sue", "Password123!", "User");
            await CreateUserIfNotExists(userManager, "galkadi", "Password123!", "Admin");
        }

        private static async Task CreateUserIfNotExists(UserManager<User> userManager, string username, string password, string role)
        {
            if (await userManager.FindByNameAsync(username) == null)
            {
                var user = new User
                {
                    UserName = username,
                    Email = $"{username}@example.com"
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
