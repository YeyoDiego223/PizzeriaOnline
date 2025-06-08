using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Tamaño",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Dimensiones = table.Column<string>(type: "TEXT", nullable: false),
                    NumeroRebanadas = table.Column<int>(type: "INTEGER", nullable: false),
                    MaximoSabores = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecioBase = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tamaño", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false),
                    PizzaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredientes_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VariantePizza",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PizzaId = table.Column<int>(type: "INTEGER", nullable: false),
                    TamañoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantePizza", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariantePizza_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VariantePizza_Tamaño_TamañoId",
                        column: x => x.TamañoId,
                        principalTable: "Tamaño",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_Ingredientes_PizzaId",
                table: "Ingredientes",
                column: "PizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_VariantePizza_PizzaId",
                table: "VariantePizza",
                column: "PizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_VariantePizza_TamañoId",
                table: "VariantePizza",
                column: "TamañoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredientes");

            migrationBuilder.DropTable(
                name: "VariantePizza");

            migrationBuilder.DropTable(
                name: "Pizzas");

            migrationBuilder.DropTable(
                name: "Tamaño");
        }
    }
}
