
using CestaFeira.Domain.Entityes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace CestaFeira.Data.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<UsuarioEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioEntity> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(p => p.Id);

            builder.HasIndex(p => p.Email).IsUnique();

            builder.HasIndex(p => new
            {
                p.Email,
                p.Senha
            });
            builder.Property(p => p.Id).HasColumnName("IdUsuario");

            builder.Property(p => p.cpf).IsRequired();

            builder.Property(p => p.Email).IsRequired().HasMaxLength(150);

            builder.Property(p => p.Senha).IsRequired();

            builder.Property(p => p.Nome).IsRequired().HasMaxLength(60);

            builder.Property(p => p.Nome).HasMaxLength(60);

            builder.Property(p => p.Cel).IsRequired();

            builder.Property(p => p.Rua).IsRequired().HasMaxLength(150);

            builder.Property(p => p.Numero).IsRequired();

            builder.Property(p => p.Bairro).IsRequired().HasMaxLength(150);

            builder.Property(p => p.Cidade).IsRequired().HasMaxLength(60);

            builder.Property(p => p.Uf).IsRequired().HasMaxLength(2);

            builder.Property(p => p.Ativo).IsRequired();

            builder.Property(p => p.Perfil).HasColumnName("Perfil");

            // Relacionamento com Produto (um usuário cadastra vários produtos)
            builder.HasMany(u => u.Produtos)
                   .WithOne(p => p.Usuario)
                   .HasForeignKey(p => p.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Pedido (um usuário faz vários pedidos)
            builder.HasMany(u => u.Pedidos)
                   .WithOne(p => p.Usuario)
                   .HasForeignKey(p => p.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

