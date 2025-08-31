using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaPromociones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Activa",
                table: "Promociones",
                newName: "EstaActiva");

            migrationBuilder.AlterColumn<string>(
                name: "RutaImagen",
                table: "Promociones",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Promociones",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Promociones");

            migrationBuilder.RenameColumn(
                name: "EstaActiva",
                table: "Promociones",
                newName: "Activa");

            migrationBuilder.AlterColumn<string>(
                name: "RutaImagen",
                table: "Promociones",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
