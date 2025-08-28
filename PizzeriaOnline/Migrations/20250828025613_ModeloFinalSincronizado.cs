using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class ModeloFinalSincronizado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallePedidos_ProductoExtras_ProductoExtraId",
                table: "DetallePedidos");

            migrationBuilder.DropIndex(
                name: "IX_DetallePedidos_ProductoExtraId",
                table: "DetallePedidos");

            migrationBuilder.AlterColumn<int>(
                name: "TamañoId",
                table: "DetallePedidos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TamañoId",
                table: "DetallePedidos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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
    }
}
