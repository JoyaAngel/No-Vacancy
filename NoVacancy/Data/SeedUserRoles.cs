using Microsoft.AspNetCore.Identity;
using NoVacancy.Models;

namespace NoVacancy.Data
{
    public class SeedUserRoles
    {

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

            // Creamos estos roles por defecto
            string[] roles = new[] { "Administrador", "Cliente" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Crear usuario administrador por defecto
            string adminEmail = "admin@novacancy.com";
            string password = "Admin123UmDelta!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new Usuario
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nombre = "Admin",
                    Rol = "Administrador"

                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Administrador");
            }
            else
            {
                // Asegura que el usuario admin tenga el rol Administrador aunque ya exista
                if (!await userManager.IsInRoleAsync(adminUser, "Administrador"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrador");
                }
            }
        }


    }
}
