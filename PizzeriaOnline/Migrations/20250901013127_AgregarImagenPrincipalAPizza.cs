using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class AgregarImagenPrincipalAPizza : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsImagenPrincipal",
                table: "PizzaImagenes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "PizzaImagenes",
                keyColumn: "Id",
                keyValue: 1,
                column: "EsImagenPrincipal",
                value: false);

            migrationBuilder.UpdateData(
                table: "PizzaImagenes",
                keyColumn: "Id",
                keyValue: 2,
                column: "EsImagenPrincipal",
                value: false);

            migrationBuilder.UpdateData(
                table: "PizzaImagenes",
                keyColumn: "Id",
                keyValue: 3,
                column: "EsImagenPrincipal",
                value: false);

            migrationBuilder.UpdateData(
                table: "PizzaImagenes",
                keyColumn: "Id",
                keyValue: 4,
                column: "EsImagenPrincipal",
                value: false);

            migrationBuilder.UpdateData(
                table: "PizzaImagenes",
                keyColumn: "Id",
                keyValue: 5,
                column: "EsImagenPrincipal",
                value: false);

            migrationBuilder.UpdateData(
                table: "PizzaImagenes",
                keyColumn: "Id",
                keyValue: 6,
                column: "EsImagenPrincipal",
                value: false);

            migrationBuilder.UpdateData(
                table: "PizzaImagenes",
                keyColumn: "Id",
                keyValue: 7,
                column: "EsImagenPrincipal",
                value: false);

            migrationBuilder.UpdateData(
                table: "PizzaImagenes",
                keyColumn: "Id",
                keyValue: 8,
                column: "EsImagenPrincipal",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsImagenPrincipal",
                table: "PizzaImagenes");
        }
    }
}
