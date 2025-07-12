using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class AddDetalleSaborRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreTamaño",
                table: "DetallePedidos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DetalleSabores",
                columns: table => new
                {
                    DetallePedidoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PizzaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleSabores", x => new { x.DetallePedidoId, x.PizzaId });
                    table.ForeignKey(
                        name: "FK_DetalleSabores_DetallePedidos_DetallePedidoId",
                        column: x => x.DetallePedidoId,
                        principalTable: "DetallePedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleSabores_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleSabores_PizzaId",
                table: "DetalleSabores",
                column: "PizzaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleSabores");

            migrationBuilder.DropColumn(
                name: "NombreTamaño",
                table: "DetallePedidos");
        }
    }
}
