using CestaFeira.Domain.Entityes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace CestaFeira.Data.Mapping
{
    public class ProdutoMap : IEntityTypeConfiguration<ProdutoEntity>
    {
        public void Configure(EntityTypeBuilder<ProdutoEntity> builder)
        {
            builder.ToTable("Produto");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).IsRequired();
            builder.Property(p => p.Descricao);
            builder.Property(p => p.quantidade).IsRequired();
            builder.Property(p => p.valorUnitario).IsRequired();

            // Relacionamento com Usuario (um produto pertence a um único usuário)
            builder.HasOne(p => p.Usuario)
                   .WithMany(u => u.Produtos)
                   .HasForeignKey(p => p.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ProdutoPedidos)  // Use ProdutoPedidos em vez de Pedidos
       .WithOne(pp => pp.Produto)
       .HasForeignKey(pp => pp.ProdutoId);

        }
    }
}
