using CestaFeira.Domain.Entityes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CestaFeira.Data.Mapping
{
    public class VendaMap : IEntityTypeConfiguration<VendaEntity>
    {
        public void Configure(EntityTypeBuilder<VendaEntity> builder)
        {
            builder.ToTable("Venda");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("IdVenda");

            builder.Property(li => li.ProdutoId).HasColumnName("IdProduto").IsRequired();
            builder.Property(li => li.UsuarioId).HasColumnName("UsuarioId").IsRequired();


        }
    }
}