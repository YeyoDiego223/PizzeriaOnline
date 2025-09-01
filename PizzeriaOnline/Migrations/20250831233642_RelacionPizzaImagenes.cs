using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class RelacionPizzaImagenes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RutaImagen",
                table: "Pizzas");

            migrationBuilder.CreateTable(
                name: "PizzaImagenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RutaImagen = table.Column<string>(type: "TEXT", nullable: false),
                    PizzaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaImagenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PizzaImagenes_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PizzaImagenes",
                columns: new[] { "Id", "PizzaId", "RutaImagen" },
                values: new object[,]
                {
                    { 1, 1, "/images/pizzas/monumental.jpg" },
                    { 2, 2, "/images/pizzas/macarena.jpg" },
                    { 3, 3, "/images/pizzas/sevillana.jpg" },
                    { 4, 4, "/images/pizzas/manoletina.jpg" },
                    { 5, 5, "/images/pizzas/miura.jpg" },
                    { 6, 6, "/images/pizzas/zapopina.jpg" },
                    { 7, 7, "/images/pizzas/chicuelina.jpg" },
                    { 8, 8, "/images/pizzas/san_fermin.jpg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PizzaImagenes_PizzaId",
                table: "PizzaImagenes",
                column: "PizzaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PizzaImagenes");

            migrationBuilder.AddColumn<string>(
                name: "RutaImagen",
                table: "Pizzas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1,
                column: "RutaImagen",
                value: "/images/pizzas/monumental.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 2,
                column: "RutaImagen",
                value: "/images/pizzas/macarena.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 3,
                column: "RutaImagen",
                value: "/images/pizzas/sevillana.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 4,
                column: "RutaImagen",
                value: "/images/pizzas/manoletina.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 5,
                column: "RutaImagen",
                value: "/images/pizzas/miura.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 6,
                column: "RutaImagen",
                value: "/images/pizzas/zapopina.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 7,
                column: "RutaImagen",
                value: "/images/pizzas/chicuelina.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 8,
                column: "RutaImagen",
                value: "/images/pizzas/san_fermin.jpg");
        }
    }
}
