using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CestaFeira.Data.Migrations
{
    public partial class terceiro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "IdUsuario",
                keyValue: new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                columns: new[] { "Data", "Senha" },
                values: new object[] { new DateTime(2024, 11, 8, 16, 30, 22, 931, DateTimeKind.Local).AddTicks(8341), "AAAAAAAAAAAAAAAAAAAAAA==.h+51QAGNHVkW3rrXIG+MpQ==.8GRwjJrWR0CrzP+jDi9N8z9irh7JXTaRNOHcnxM6M8U=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Pedido");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "IdUsuario",
                keyValue: new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                columns: new[] { "Data", "Senha" },
                values: new object[] { new DateTime(2024, 11, 4, 13, 0, 1, 736, DateTimeKind.Local).AddTicks(6255), "AAAAAAAAAAAAAAAAAAAAAA==.GXzOkBeS7nbiDBi+hvntpg==.J/EDFfIIzsx/JWYKkmAd2SNntt0VEJTtxz7pLuDexFA=" });
        }
    }
}
