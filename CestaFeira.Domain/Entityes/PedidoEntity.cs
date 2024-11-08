using CestaFeira.Domain.Entityes.Base;

namespace CestaFeira.Domain.Entityes
{
    public class PedidoEntity:BaseEntity
    {
        public DateTime Data { get; set; }
        public Guid UsuarioId { get; set; }
        public UsuarioEntity Usuario { get; set; }
        public string Status { get; set; }
        public ICollection<PedidoProdutoEntity> ProdutoPedidos { get; set; }
    }
}
