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

            // --- PRIMERO: DEFINIMOS LA PRECISIÓN DE LOS DECIMALES (SIN COMENTARIOS) ---
            modelBuilder.Entity<Configuracion>()
                .Property(p => p.PromocionPrecio)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<DetallePedido>()
                .Property(p => p.PrecioUnitario)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Ingrediente>()
                .Property(p => p.CantidadEnStock)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.TotalPedido)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ProductoExtra>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Receta>()
                .Property(p => p.Cantidad)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Tamaño>()
                .Property(p => p.PrecioBase)
                .HasColumnType("decimal(18, 2)");


            // --- SEGUNDO: AÑADIMOS LOS DATOS INICIALES (SEED DATA) ---
           
            modelBuilder.Entity<Tamaño>().HasData(
                new Tamaño { Id = 1, Nombre = "Mediana", Dimensiones = "35cm", NumeroRebanadas = 6, PrecioBase = 150.00m, MaximoSabores = 1 },
                new Tamaño { Id = 2, Nombre = "Grande", Dimensiones = "40cm", NumeroRebanadas = 8, PrecioBase = 200.00m, MaximoSabores = 2 },
                new Tamaño { Id = 3, Nombre = "Familiar", Dimensiones = "45cm", NumeroRebanadas = 12, PrecioBase = 250.00m, MaximoSabores = 2 },
                new Tamaño { Id = 4, Nombre = "Cuadrada", Dimensiones = "50cm", NumeroRebanadas = 16, PrecioBase = 350.00m, MaximoSabores = 4 },
                new Tamaño { Id = 5, Nombre = "Rectangular", Dimensiones = "65cm", NumeroRebanadas = 24, PrecioBase = 500.00m, MaximoSabores = 4 }
            );

            modelBuilder.Entity<Pizza>().HasData(
                new Pizza { Id = 1, Nombre = "Monumental", Descripcion = "Masa maestra, salsa secreta, aceitunas negras, morrón, elote, cebolla morada, pepperoni, piña, champiñones, romero, mozzarella" },
                new Pizza { Id = 2, Nombre = "Macarena", Descripcion = "Masa maestra, salsa secreta, queso mozzarella, albhaca fresca." },
                new Pizza { Id = 3, Nombre = "Sevillana", Descripcion = "Masa maestra, salsa secreta, piña, jamón, mozzarella" },
                new Pizza { Id = 4, Nombre = "Manoletina", Descripcion = "Masa maestra, salsa secreta, pepperoni , mozzarella" },
                new Pizza { Id = 5, Nombre = "Miura", Descripcion = "Masa maestra, salsa secreta, frijoles, elote, morrón, tocino, chorizo, rodajas de serrano" },
                new Pizza { Id = 6, Nombre = "Zapopina", Descripcion = "Masa maestra salsa secreta, carne de alambre, mozzarella" },
                new Pizza { Id = 7, Nombre = "Chicuelina", Descripcion = "Masa maestra, salsa secreta, queso chihuaha, queso monterrey jack, queso cheddar, queso asadero, queso mozzarela" },
                new Pizza { Id = 8, Nombre = "San Fermin", Descripcion = "Masa maestra, salsa secreta, carne al pastor, mozzarella" }
            );

            modelBuilder.Entity<PizzaImagen>().HasData(
                new PizzaImagen { Id = 1, PizzaId = 1, RutaImagen = "/images/pizzas/monumental.jpg", EsImagenPrincipal = true },
                new PizzaImagen { Id = 2, PizzaId = 2, RutaImagen = "/images/pizzas/macarena.jpg", EsImagenPrincipal = true },
                new PizzaImagen { Id = 3, PizzaId = 3, RutaImagen = "/images/pizzas/sevillana.jpg", EsImagenPrincipal = true },
                new PizzaImagen { Id = 4, PizzaId = 4, RutaImagen = "/images/pizzas/manoletina.jpg", EsImagenPrincipal = true },
                new PizzaImagen { Id = 5, PizzaId = 5, RutaImagen = "/images/pizzas/miura.jpg", EsImagenPrincipal = true },
                new PizzaImagen { Id = 6, PizzaId = 6, RutaImagen = "/images/pizzas/zapopina.jpg", EsImagenPrincipal = true },
                new PizzaImagen { Id = 7, PizzaId = 7, RutaImagen = "/images/pizzas/chicuelina.jpg", EsImagenPrincipal = true },
                new PizzaImagen { Id = 8, PizzaId = 8, RutaImagen = "/images/pizzas/san_fermin.jpg", EsImagenPrincipal = true }
            );

            modelBuilder.Entity<Configuracion>().HasData(
                new Configuracion
                {
                    Id = 1,
                    ForzarCierre = false,
                    HoraApertura = new TimeSpan(14, 0, 0),
                    HoraCierre = new TimeSpan(22, 0, 0),
                    PromocionEstaActiva = false, // Valores por defecto para la nueva promo
                    PromocionTitulo = "",
                    PromocionDescripcion = "",
                    PromocionPrecio = 0,
                    PromocionRutaImagen = ""
                }
            );

            // --- TERCERO: CONFIGURACIÓN DE CLAVES COMPUESTAS Y RELACIONES ---
            modelBuilder.Entity<Receta>()
                .HasKey(r => new { r.PizzaId, r.TamañoId, r.IngredienteId });

            modelBuilder.Entity<DetalleSabor>()
                .HasKey(ds => new { ds.DetallePedidoId, ds.PizzaId });

            modelBuilder.Entity<DetalleSabor>()
                .HasOne(ds => ds.DetallePedido)
                .WithMany(dp => dp.DetalleSabores)
                .HasForeignKey(ds => ds.DetallePedidoId);

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
        public DbSet<DetalleSabor> DetalleSabores { get; set; }
        public DbSet<Receta> Recetas { get; set; }

        public DbSet<ProductoExtra> ProductoExtras { get; set; }

        public DbSet<MensajeChat> MensajesChat { get; set; }
        public DbSet<PizzaImagen> PizzaImagenes { get; set; }

        public DbSet<Configuracion> Configuracion { get; set; }
    }

}
    
