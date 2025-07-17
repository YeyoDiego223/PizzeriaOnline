using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class AddTamañoIdToDetallePedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TamañoId",
                table: "DetallePedidos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Tamaños",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nombre",
                value: "Familiar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TamañoId",
                table: "DetallePedidos");

            migrationBuilder.UpdateData(
                table: "Tamaños",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nombre",
                value: "Mediana");
        }
    }
}
