using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CestaFeira.Data.Migrations
{
    public partial class segundo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeFantasia",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "IdUsuario",
                keyValue: new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                columns: new[] { "Data", "NomeFantasia", "Senha" },
                values: new object[] { new DateTime(2024, 11, 4, 13, 0, 1, 736, DateTimeKind.Local).AddTicks(6255), "", "AAAAAAAAAAAAAAAAAAAAAA==.GXzOkBeS7nbiDBi+hvntpg==.J/EDFfIIzsx/JWYKkmAd2SNntt0VEJTtxz7pLuDexFA=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeFantasia",
                table: "Usuario");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "IdUsuario",
                keyValue: new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                columns: new[] { "Data", "Senha" },
                values: new object[] { new DateTime(2024, 10, 21, 15, 45, 12, 733, DateTimeKind.Local).AddTicks(5043), "AAAAAAAAAAAAAAAAAAAAAA==.rH5VXabL6YW0kkaRVNBGEw==.evUpyJ+DH7YU7gByvqB/ywImWj2wMDLhkoVnxPWTbP0=" });
        }
    }
}
