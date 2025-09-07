using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaOnline.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaConfiguracionConDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Configuracion",
                columns: new[] { "Id", "ForzarCierre", "HoraApertura", "HoraCierre" },
                values: new object[] { 1, false, new TimeSpan(0, 14, 0, 0, 0), new TimeSpan(0, 22, 0, 0, 0) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Configuracion",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
