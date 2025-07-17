using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class SeedPizzaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
