using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class AddCantidadEnStockIngredientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "Ingredientes",
                newName: "UnidadDeMedida");

            migrationBuilder.AddColumn<double>(
                name: "CantidadEnStock",
                table: "Ingredientes",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadEnStock",
                table: "Ingredientes");

            migrationBuilder.RenameColumn(
                name: "UnidadDeMedida",
                table: "Ingredientes",
                newName: "Precio");
        }
    }
}
