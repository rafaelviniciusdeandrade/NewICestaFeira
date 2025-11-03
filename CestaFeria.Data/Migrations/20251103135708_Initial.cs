using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CestaFeira.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoPagamento",
                table: "Pedido",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "IdUsuario",
                keyValue: new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                columns: new[] { "Data", "NomeFantasia", "Senha" },
                values: new object[] { new DateTime(2025, 11, 3, 10, 57, 7, 445, DateTimeKind.Local).AddTicks(9409), "Teste", "AAAAAAAAAAAAAAAAAAAAAA==.6zFPxqnRy/8DYRasHfJztQ==.AYL6K2QdJTw/XjDFcEiyCAjMR1zWB00pnf1mqLMpT/w=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoPagamento",
                table: "Pedido");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "IdUsuario",
                keyValue: new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                columns: new[] { "Data", "NomeFantasia", "Senha" },
                values: new object[] { new DateTime(2024, 11, 18, 16, 16, 27, 18, DateTimeKind.Local).AddTicks(9488), "", "AAAAAAAAAAAAAAAAAAAAAA==.KQQXlYxdzG9eK2531SXUEg==.ehM/BL/Y3GjhPAXNspbr1x3qFiaDZnAR7pWq5sF8t1s=" });
        }
    }
}
