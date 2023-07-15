using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillageAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarVilllage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villages",
                columns: new[] { "Id", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadros", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "Detalle de la Villa..", new DateTime(2023, 7, 14, 13, 7, 35, 647, DateTimeKind.Local).AddTicks(6560), new DateTime(2023, 7, 14, 13, 7, 35, 647, DateTimeKind.Local).AddTicks(6550), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "Detalle de la Villa..", new DateTime(2023, 7, 14, 13, 7, 35, 647, DateTimeKind.Local).AddTicks(6562), new DateTime(2023, 7, 14, 13, 7, 35, 647, DateTimeKind.Local).AddTicks(6561), "", 40, "Premium vista a la piscina", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villages",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
