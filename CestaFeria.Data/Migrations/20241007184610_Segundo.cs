using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CestaFeira.Data.Migrations
{
    public partial class Segundo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Usuario_Id",
                table: "Produto");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Produto",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte[]>(
                name: "imagem",
                table: "Produto",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "IdUsuario",
                keyValue: new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                columns: new[] { "Data", "Senha" },
                values: new object[] { new DateTime(2024, 10, 7, 15, 46, 9, 954, DateTimeKind.Local).AddTicks(1325), "AAAAAAAAAAAAAAAAAAAAAA==.N0Kq16apxPJANEhJnk99+Q==.AR6TGFnbGUzhlEBjImiaZlyNkMeFj9PgAHKTHhctueA=" });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_UsuarioId",
                table: "Produto",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Usuario_UsuarioId",
                table: "Produto",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Usuario_UsuarioId",
                table: "Produto");

            migrationBuilder.DropIndex(
                name: "IX_Produto_UsuarioId",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "imagem",
                table: "Produto");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "IdUsuario",
                keyValue: new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                columns: new[] { "Data", "Senha" },
                values: new object[] { new DateTime(2024, 10, 4, 16, 17, 37, 605, DateTimeKind.Local).AddTicks(6541), "AAAAAAAAAAAAAAAAAAAAAA==.T1a3AOlRpoS7mJDN2+htIQ==.Na7qDuFsYKjustyJz8f79fZmFyI6Ah/FKRYiRkZaR6w=" });

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Usuario_Id",
                table: "Produto",
                column: "Id",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
