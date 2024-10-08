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

            builder.HasOne(p => p.Usuario)
              .WithMany(p => p.Produtos)
              .HasForeignKey(p => p.UsuarioId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
