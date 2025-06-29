using Microsoft.AspNetCore.Identity;
using TFG.Domain.Entities;

namespace TFG.Infrastructure.Data
{
    public static class AdminUserSeeder
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            // Solo si no existe ningún usuario
            if (dbContext.Users.Any())
                return;

            var adminSection = configuration.GetSection("AdminUser");
            var adminEmail = adminSection["Email"] ?? "admin@admin.com";
            var adminUserName = adminSection["Email"] ?? "admin@admin.com";
            var adminPassword = adminSection["Password"] ?? "Admin123!";
            var adminFirstName = adminSection["FirstName"] ?? "Admin";
            var adminLastName = adminSection["LastName"] ?? "User";

            var adminUser = new User
            {
                Email = adminEmail,
                EmailConfirmed = true,
                UserName = adminUserName,
                IsAdmin = true,
                FirstName = adminFirstName,
                LastName = adminLastName,
                OpenProjectId = string.Empty,
                GitlabId = string.Empty,
                SonarQubeId = string.Empty,
            };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
