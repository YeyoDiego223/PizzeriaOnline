using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
namespace PizzeriaOnline.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            

            modelBuilder.Entity<Tamaño>().HasData(
                new Tamaño { Id = 1, Nombre = "Mediana", Dimensiones = "35cm", NumeroRebanadas = 6, PrecioBase = 150.00m, MaximoSabores = 1 },
                new Tamaño { Id = 2, Nombre = "Grande", Dimensiones = "40cm", NumeroRebanadas = 8, PrecioBase = 200.00m, MaximoSabores = 2 },
                new Tamaño { Id = 3, Nombre = "Mediana", Dimensiones = "45cm", NumeroRebanadas = 12, PrecioBase = 250.00m, MaximoSabores = 2 },
                new Tamaño { Id = 4, Nombre = "Cuadrada", Dimensiones = "50cm", NumeroRebanadas = 16, PrecioBase = 350.00m, MaximoSabores = 4 },
                new Tamaño { Id = 5, Nombre = "Rectangular", Dimensiones = "65cm", NumeroRebanadas = 24, PrecioBase = 500.00m, MaximoSabores = 4 }

                );

            modelBuilder.Entity<PizzaIngrediente>()
                .HasKey(pi => new { pi.PizzaId, pi.IngredienteId });

            modelBuilder.Entity<DetalleSabor>()
                .HasKey(ds => new { ds.DetallePedidoId, ds.PizzaId });

            // Define la relación entre DetalleSabor y DetallePedido
            modelBuilder.Entity<DetalleSabor>()
                .HasOne(ds => ds.DetallePedido)
                .WithMany(dp => dp.DetalleSabores)
                .HasForeignKey(ds => ds.DetallePedidoId);

            // Define la relación entre DetalleSabor y Pizza
            modelBuilder.Entity<DetalleSabor>()
                .HasOne(ds => ds.Pizza)
                .WithMany(p => p.DetalleSabores)
                .HasForeignKey(ds => ds.PizzaId);
        }      

        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<Tamaño> Tamaños { get; set; }
        
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
        public DbSet<PizzaIngrediente> PizzaIngredientes { get; set; }
        public DbSet<DetalleSabor> DetalleSabores { get; set; }
    }

}
    
