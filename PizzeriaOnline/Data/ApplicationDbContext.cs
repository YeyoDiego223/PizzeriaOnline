using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Models;
using System.Collections.Generic;
namespace PizzeriaOnline.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pizza>().HasData(
            new Pizza
            {
                Id = 1,
                Nombre = "Monumental",
                Descripcion = "Masa maestra, salsa secreta, aceitunas negras, morrón, elote, cebolla morada, pepperoni, piña, champiñones, romero, mozzarella",
                RutaImagen = "/images/pizzas/monumental.jpg" // Asumimos que tendremos esta imagen más tarde
            },
            new Pizza
            {
                Id = 2,
                Nombre = "Macarena",
                Descripcion = "Masa maestra, salsa secreta, queso mozzarella, albhaca fresca.",
                RutaImagen = "/images/pizzas/macarena.jpg"
            },
            new Pizza
            {
                Id = 3,
                Nombre = "Sevillana",
                Descripcion = "Masa maestra, salsa secreta, piña, jamón, mozzarella",
                RutaImagen = "/images/pizzas/sevillana.jpg"
            },
            new Pizza
            {
                Id = 4,
                Nombre = "Manoletina",
                Descripcion = "Masa maestra, salsa secreta, pepperoni , mozzarella",
                RutaImagen = "/images/pizzas/manoletina.jpg"
            },
            new Pizza
            {
                Id = 5,
                Nombre = "Miura",
                Descripcion = "Masa maestra, salsa secreta, frijoles, elote, morrón, tocino, chorizo, rodajas de serrano",
                RutaImagen = "/images/pizzas/miura.jpg"
            },
            new Pizza
            {
                Id = 6,
                Nombre = "Zapopina",
                Descripcion = "Masa maestra salsa secreta, carne de alambre, mozzarella",
                RutaImagen = "/images/pizzas/zapopina.jpg"
            },
            new Pizza
            {
                Id = 7,
                Nombre = "Chicuelina",
                Descripcion = "Masa maestra, salsa secreta, queso chihuaha, queso monterrey jack, queso cheddar, queso asadero, queso mozzarela",
                RutaImagen = "/images/pizzas/chicuelina.jpg"
            },
            new Pizza
            {
                Id= 8,
                Nombre = "San Fermin",
                Descripcion = "Masa maestra, salsa secreta, carne al pastor, mozzarella",
                RutaImagen = "/images/pizzas/san_fermin.jpg"
            }
            );
        }      

        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Ingredientes> Ingredientes { get; set; }
    }

}
    
