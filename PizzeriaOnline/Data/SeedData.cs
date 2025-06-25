// Data/SeedData.cs
using Microsoft.AspNetCore.Identity;

namespace PizzeriaOnline.Data
{
    public static class SeedData
    {
        // Este método se encargará de toda la inicialización
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // --- Creación de Roles ---
            string[] roleNames = { "Admin", "Cliente" };
            foreach (var roleName in roleNames)
            {
                // Verificamos si el rol ya existe
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Si no existe, lo creamos
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // --- Creación del Usuario Administrador ---
            var adminEmail = "admin@pizzeria.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // Si no encontramos un usuario con ese email...
            if (adminUser == null)
            {
                // ...creamos uno nuevo
                var newAdminUser = new IdentityUser()
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true, // Lo confirmamos para no tener que hacerlo por email
                };

                // Usamos el userManager para crear el usuario con una contraseña
                var result = await userManager.CreateAsync(newAdminUser, "Password123!");

                if (result.Succeeded)
                {
                    // Si se creó con éxito, le asignamos el rol de "Admin"
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                }
            }
        }
    }
}