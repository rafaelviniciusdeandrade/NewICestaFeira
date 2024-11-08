using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CestaFeira.Data.Mapping
{
    public class PedidoMap : IEntityTypeConfiguration<Domain.Entityes.PedidoEntity>
    {
        public void Configure(EntityTypeBuilder<Domain.Entityes.PedidoEntity> builder)
        {
            builder.ToTable("Pedido");

            // Definir a chave primária
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("IdPedido");
            builder.Property(p => p.Status).HasColumnName("Status");
            // Definir propriedades obrigatórias
            builder.Property(p => p.UsuarioId).HasColumnName("UsuarioId").IsRequired();
            builder.Property(p => p.Data).IsRequired();

            // Relacionamento com o Usuario (um usuário pode fazer muitos pedidos)
            builder.HasOne(p => p.Usuario)
                   .WithMany(u => u.Pedidos)
                   .HasForeignKey(p => p.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento muitos-para-muitos com Produto via entidade de junção
            builder.HasMany(p => p.ProdutoPedidos)   // Um pedido pode ter muitos produtos (via PedidoProdutoEntity)
                   .WithOne(pp => pp.Pedido)        // Cada PedidoProdutoEntity está relacionado a um Pedido
                   .HasForeignKey(pp => pp.PedidoId);
        }
    }
}
