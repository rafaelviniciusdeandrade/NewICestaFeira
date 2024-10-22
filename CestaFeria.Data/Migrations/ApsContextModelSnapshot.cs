﻿// <auto-generated />
using System;
using CestaFeria.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CestaFeira.Data.Migrations
{
    [DbContext(typeof(ApsContext))]
    partial class ApsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CestaFeira.Domain.Entityes.PedidoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdPedido");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UsuarioId");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Pedido", (string)null);
                });

            modelBuilder.Entity("CestaFeira.Domain.Entityes.PedidoProdutoEntity", b =>
                {
                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.HasKey("ProdutoId", "PedidoId");

                    b.HasIndex("PedidoId");

                    b.ToTable("PedidoProduto", (string)null);
                });

            modelBuilder.Entity("CestaFeira.Domain.Entityes.ProdutoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("imagem")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("quantidade")
                        .HasColumnType("int");

                    b.Property<double>("valorUnitario")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Produto", (string)null);
                });

            modelBuilder.Entity("CestaFeira.Domain.Entityes.UsuarioEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdUsuario");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit")
                        .HasColumnName("Ativo");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("Bairro");

                    b.Property<string>("Cel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Cel");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("Cidade");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2")
                        .HasColumnName("Data");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("Email");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("Nome");

                    b.Property<int>("Numero")
                        .HasColumnType("int")
                        .HasColumnName("Numero");

                    b.Property<string>("Perfil")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Perfil");

                    b.Property<string>("Rua")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("Rua");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Senha");

                    b.Property<string>("Uf")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)")
                        .HasColumnName("Uf");

                    b.Property<string>("cpf")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("cpf");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Email", "Senha");

                    b.ToTable("Usuario", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                            Ativo = true,
                            Bairro = "Jardim São Carlos",
                            Cel = "(35)11111111",
                            Cidade = "Alfenas",
                            Data = new DateTime(2024, 10, 21, 15, 45, 12, 733, DateTimeKind.Local).AddTicks(5043),
                            Email = "rafael@gmail.com",
                            Nome = "Administrador",
                            Numero = 555,
                            Perfil = "ADM",
                            Rua = "Juscelino Kubitschek",
                            Senha = "AAAAAAAAAAAAAAAAAAAAAA==.rH5VXabL6YW0kkaRVNBGEw==.evUpyJ+DH7YU7gByvqB/ywImWj2wMDLhkoVnxPWTbP0=",
                            Uf = "MG",
                            cpf = "13080460812"
                        });
                });

            modelBuilder.Entity("CestaFeira.Domain.Entityes.PedidoEntity", b =>
                {
                    b.HasOne("CestaFeira.Domain.Entityes.UsuarioEntity", "Usuario")
                        .WithMany("Pedidos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("CestaFeira.Domain.Entityes.PedidoProdutoEntity", b =>
                {
                    b.HasOne("CestaFeira.Domain.Entityes.PedidoEntity", "Pedido")
                        .WithMany("ProdutoPedidos")
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CestaFeira.Domain.Entityes.ProdutoEntity", "Produto")
                        .WithMany("ProdutoPedidos")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("CestaFeira.Domain.Entityes.ProdutoEntity", b =>
                {
                    b.HasOne("CestaFeira.Domain.Entityes.UsuarioEntity", "Usuario")
                        .WithMany("Produtos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("CestaFeira.Domain.Entityes.PedidoEntity", b =>
                {
                    b.Navigation("ProdutoPedidos");
                });

            modelBuilder.Entity("CestaFeira.Domain.Entityes.ProdutoEntity", b =>
                {
                    b.Navigation("ProdutoPedidos");
                });

            modelBuilder.Entity("CestaFeira.Domain.Entityes.UsuarioEntity", b =>
                {
                    b.Navigation("Pedidos");

                    b.Navigation("Produtos");
                });
#pragma warning restore 612, 618
        }
    }
}
