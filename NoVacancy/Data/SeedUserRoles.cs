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

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var user = new Usuario
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nombre = "Admin",
                    Rol = "Administrador",
                    Calle = "Av. Universidad",
                    Numero = "789",
                    Colonia = "Copilco",
                    Ciudad = "CDMX",
                    Estado = "CDMX",
                    CodigoPostal = "04360"
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Administrador");
            }

            //Crear otro admin (prueba del seeder).
            string adminEmail2 = "chucho@novacancy.com";
            string password2 = "AlephNull0.";
            if(await userManager.FindByEmailAsync(adminEmail2) == null)
            {
                var user2 = new Usuario
                {
                    UserName = adminEmail2,
                    Email = adminEmail2,
                    Nombre = "Chucho",
                    Rol = "Administrador",
                    Calle = "Av. Siempre Viva",
                    Numero = "742",
                    Colonia = "Springfield",
                    Ciudad = "CDMX",
                    Estado = "CDMX",
                    CodigoPostal = "12345"
                };

                var result2 = await userManager.CreateAsync(user2, password2);
                if (result2.Succeeded)
                    await userManager.AddToRoleAsync(user2, "Administrador");
            }

            // Crear usuario cliente "Chi"
            string chiEmail = "joya.mag02@gmail.com";
            string chiPassword = "ChiCliente123!";
            if(await userManager.FindByEmailAsync(chiEmail) == null)
            {
                var chiUser = new Usuario
                {
                    UserName = chiEmail,
                    Email = chiEmail,
                    Nombre = "Chi",
                    Rol = "Cliente",
                    Calle = "Calle Chi",
                    Numero = "1",
                    Colonia = "Colonia Chi",
                    Ciudad = "CDMX",
                    Estado = "CDMX",
                    CodigoPostal = "01000"
                };
                var chiResult = await userManager.CreateAsync(chiUser, chiPassword);
                if (chiResult.Succeeded)
                    await userManager.AddToRoleAsync(chiUser, "Cliente");
            }

        }


    }
}
