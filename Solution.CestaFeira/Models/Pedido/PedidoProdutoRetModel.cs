using CestaFeira.Web.Models.Usuario;

namespace CestaFeira.Web.Models.Pedido
{
    public class PedidoProdutoRetModel
    {
        public Guid IdPedido { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime Data { get; set; }
        public List<PedidoProdutoModel> PedidoProdutos { get; set; }
        public string Status { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
