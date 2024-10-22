using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace CestaFeira.Data.Mapping
{
    public class PedidoProdutoMap : IEntityTypeConfiguration<Domain.Entityes.PedidoProdutoEntity>
    {
        public void Configure(EntityTypeBuilder<Domain.Entityes.PedidoProdutoEntity> builder)
        {
            builder.ToTable("PedidoProduto");
            builder.HasKey(pp => new { pp.ProdutoId, pp.PedidoId }); // Chave composta

            // Relação com Produto
            builder.HasOne(pp => pp.Produto)
                   .WithMany(p => p.ProdutoPedidos)
                   .HasForeignKey(pp => pp.ProdutoId);

            // Relação com Pedido
            builder.HasOne(pp => pp.Pedido)
                   .WithMany(p => p.ProdutoPedidos)
                   .HasForeignKey(pp => pp.PedidoId);
        }
    }
}
