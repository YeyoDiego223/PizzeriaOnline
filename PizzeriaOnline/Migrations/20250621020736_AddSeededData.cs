using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class AddSeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pizzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    RutaImagen = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pizzas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tamaños",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Dimensiones = table.Column<string>(type: "TEXT", nullable: false),
                    NumeroRebanadas = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecioBase = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaximoSabores = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tamaños", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Pizzas",
                columns: new[] { "Id", "Descripcion", "Nombre", "RutaImagen" },
                values: new object[,]
                {
                    { 1, "Masa maestra, salsa secreta, aceitunas negras, morrón, elote, cebolla morada, pepperoni, piña, champiñones, romero, mozzarella", "Monumental", "/images/pizzas/monumental.jpg" },
                    { 2, "Masa maestra, salsa secreta, queso mozzarella, albhaca fresca.", "Macarena", "/images/pizzas/macarena.jpg" },
                    { 3, "Masa maestra, salsa secreta, piña, jamón, mozzarella", "Sevillana", "/images/pizzas/sevillana.jpg" },
                    { 4, "Masa maestra, salsa secreta, pepperoni , mozzarella", "Manoletina", "/images/pizzas/manoletina.jpg" },
                    { 5, "Masa maestra, salsa secreta, frijoles, elote, morrón, tocino, chorizo, rodajas de serrano", "Miura", "/images/pizzas/miura.jpg" },
                    { 6, "Masa maestra salsa secreta, carne de alambre, mozzarella", "Zapopina", "/images/pizzas/zapopina.jpg" },
                    { 7, "Masa maestra, salsa secreta, queso chihuaha, queso monterrey jack, queso cheddar, queso asadero, queso mozzarela", "Chicuelina", "/images/pizzas/chicuelina.jpg" },
                    { 8, "Masa maestra, salsa secreta, carne al pastor, mozzarella", "San Fermin", "/images/pizzas/san_fermin.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Tamaños",
                columns: new[] { "Id", "Dimensiones", "MaximoSabores", "Nombre", "NumeroRebanadas", "PrecioBase" },
                values: new object[,]
                {
                    { 1, "35cm", 1, "Mediana", 6, 150.00m },
                    { 2, "40cm", 2, "Grande", 8, 200.00m },
                    { 3, "45cm", 2, "Mediana", 12, 250.00m },
                    { 4, "50cm", 4, "Cuadrada", 16, 350.00m },
                    { 5, "65cm", 4, "Rectangular", 24, 500.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredientes");

            migrationBuilder.DropTable(
                name: "Pizzas");

            migrationBuilder.DropTable(
                name: "Tamaños");
        }
    }
}
