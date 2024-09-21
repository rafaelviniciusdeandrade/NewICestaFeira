using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CestaFeira.Data.Migrations
{
    public partial class segundo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Vendas_VendaEntityId",
                table: "Produto");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Usuario_UsuarioId",
                table: "Vendas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendas",
                table: "Vendas");

            migrationBuilder.RenameTable(
                name: "Vendas",
                newName: "Venda");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Venda",
                newName: "IdVenda");

            migrationBuilder.RenameIndex(
                name: "IX_Vendas_UsuarioId",
                table: "Venda",
                newName: "IX_Venda_UsuarioId");

            migrationBuilder.AddColumn<Guid>(
                name: "IdProduto",
                table: "Venda",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venda",
                table: "Venda",
                column: "IdVenda");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "IdUsuario",
                keyValue: new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                columns: new[] { "Data", "Senha" },
                values: new object[] { new DateTime(2024, 9, 19, 14, 5, 19, 655, DateTimeKind.Local).AddTicks(6363), "AAAAAAAAAAAAAAAAAAAAAA==.JrQ1GZ8Y6ybghXMFCxJH6g==.iGDYeURie0KQu2mY4gzv1+JsNuM87gMFl3oKhQl17WE=" });

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Venda_VendaEntityId",
                table: "Produto",
                column: "VendaEntityId",
                principalTable: "Venda",
                principalColumn: "IdVenda");

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Usuario_UsuarioId",
                table: "Venda",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Venda_VendaEntityId",
                table: "Produto");

            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Usuario_UsuarioId",
                table: "Venda");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venda",
                table: "Venda");

            migrationBuilder.DropColumn(
                name: "IdProduto",
                table: "Venda");

            migrationBuilder.RenameTable(
                name: "Venda",
                newName: "Vendas");

            migrationBuilder.RenameColumn(
                name: "IdVenda",
                table: "Vendas",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Venda_UsuarioId",
                table: "Vendas",
                newName: "IX_Vendas_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendas",
                table: "Vendas",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "IdUsuario",
                keyValue: new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                columns: new[] { "Data", "Senha" },
                values: new object[] { new DateTime(2024, 9, 19, 11, 33, 50, 783, DateTimeKind.Local).AddTicks(9402), "AAAAAAAAAAAAAAAAAAAAAA==.XX7jD2oxYNzQ1dN5QTb3mg==.8brshsL5uJMEmxK/bjA2p3rva5lNSmNTEwq5KVDM4S0=" });

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Vendas_VendaEntityId",
                table: "Produto",
                column: "VendaEntityId",
                principalTable: "Vendas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Usuario_UsuarioId",
                table: "Vendas",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
