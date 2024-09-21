using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CestaFeira.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Cel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rua = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Uf = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Perfil = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendas_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    valorUnitario = table.Column<double>(type: "float", nullable: false),
                    VendaEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_Usuario_Id",
                        column: x => x.Id,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Produto_Vendas_VendaEntityId",
                        column: x => x.VendaEntityId,
                        principalTable: "Vendas",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Ativo", "Bairro", "Cel", "Cidade", "Data", "Email", "Nome", "Numero", "Perfil", "Rua", "Senha", "Uf", "cpf" },
                values: new object[] { new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"), true, "Jardim São Carlos", "(35)11111111", "Alfenas", new DateTime(2024, 9, 19, 11, 33, 50, 783, DateTimeKind.Local).AddTicks(9402), "rafael@gmail.com", "Administrador", 555, "ADM", "Juscelino Kubitschek", "AAAAAAAAAAAAAAAAAAAAAA==.XX7jD2oxYNzQ1dN5QTb3mg==.8brshsL5uJMEmxK/bjA2p3rva5lNSmNTEwq5KVDM4S0=", "MG", "13080460812" });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_VendaEntityId",
                table: "Produto",
                column: "VendaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email_Senha",
                table: "Usuario",
                columns: new[] { "Email", "Senha" });

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_UsuarioId",
                table: "Vendas",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
