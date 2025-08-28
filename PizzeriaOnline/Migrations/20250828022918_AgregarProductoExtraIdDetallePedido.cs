using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProductoExtraIdDetallePedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductoExtraId",
                table: "DetallePedidos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedidos_ProductoExtraId",
                table: "DetallePedidos",
                column: "ProductoExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallePedidos_ProductoExtras_ProductoExtraId",
                table: "DetallePedidos",
                column: "ProductoExtraId",
                principalTable: "ProductoExtras",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallePedidos_ProductoExtras_ProductoExtraId",
                table: "DetallePedidos");

            migrationBuilder.DropIndex(
                name: "IX_DetallePedidos_ProductoExtraId",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "ProductoExtraId",
                table: "DetallePedidos");
        }
    }
}
