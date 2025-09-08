using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class SimplificarPromocionesEnConfiguracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promociones");

            migrationBuilder.AddColumn<decimal>(
                name: "PrmocionPrecio",
                table: "Configuracion",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromocionDescripcion",
                table: "Configuracion",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromocionRutaImagen",
                table: "Configuracion",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromocionTitulo",
                table: "Configuracion",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Configuracion",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PrmocionPrecio", "PromocionDescripcion", "PromocionRutaImagen", "PromocionTitulo" },
                values: new object[] { null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrmocionPrecio",
                table: "Configuracion");

            migrationBuilder.DropColumn(
                name: "PromocionDescripcion",
                table: "Configuracion");

            migrationBuilder.DropColumn(
                name: "PromocionRutaImagen",
                table: "Configuracion");

            migrationBuilder.DropColumn(
                name: "PromocionTitulo",
                table: "Configuracion");

            migrationBuilder.CreateTable(
                name: "Promociones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    EstaActiva = table.Column<bool>(type: "INTEGER", nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false),
                    RutaImagen = table.Column<string>(type: "TEXT", nullable: true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promociones", x => x.Id);
                });
        }
    }
}
