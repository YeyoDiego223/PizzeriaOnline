using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSetsForTamañoAndVariantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariantePizza_Pizzas_PizzaId",
                table: "VariantePizza");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantePizza_Tamaño_TamañoId",
                table: "VariantePizza");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariantePizza",
                table: "VariantePizza");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tamaño",
                table: "Tamaño");

            migrationBuilder.RenameTable(
                name: "VariantePizza",
                newName: "VariantePizzas");

            migrationBuilder.RenameTable(
                name: "Tamaño",
                newName: "Tamaños");

            migrationBuilder.RenameIndex(
                name: "IX_VariantePizza_TamañoId",
                table: "VariantePizzas",
                newName: "IX_VariantePizzas_TamañoId");

            migrationBuilder.RenameIndex(
                name: "IX_VariantePizza_PizzaId",
                table: "VariantePizzas",
                newName: "IX_VariantePizzas_PizzaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariantePizzas",
                table: "VariantePizzas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tamaños",
                table: "Tamaños",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariantePizzas_Pizzas_PizzaId",
                table: "VariantePizzas",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantePizzas_Tamaños_TamañoId",
                table: "VariantePizzas",
                column: "TamañoId",
                principalTable: "Tamaños",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariantePizzas_Pizzas_PizzaId",
                table: "VariantePizzas");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantePizzas_Tamaños_TamañoId",
                table: "VariantePizzas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariantePizzas",
                table: "VariantePizzas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tamaños",
                table: "Tamaños");

            migrationBuilder.RenameTable(
                name: "VariantePizzas",
                newName: "VariantePizza");

            migrationBuilder.RenameTable(
                name: "Tamaños",
                newName: "Tamaño");

            migrationBuilder.RenameIndex(
                name: "IX_VariantePizzas_TamañoId",
                table: "VariantePizza",
                newName: "IX_VariantePizza_TamañoId");

            migrationBuilder.RenameIndex(
                name: "IX_VariantePizzas_PizzaId",
                table: "VariantePizza",
                newName: "IX_VariantePizza_PizzaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariantePizza",
                table: "VariantePizza",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tamaño",
                table: "Tamaño",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariantePizza_Pizzas_PizzaId",
                table: "VariantePizza",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantePizza_Tamaño_TamañoId",
                table: "VariantePizza",
                column: "TamañoId",
                principalTable: "Tamaño",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
