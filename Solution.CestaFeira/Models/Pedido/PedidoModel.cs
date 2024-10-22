using CestaFeira.Web.Models.Produto;

namespace CestaFeira.Web.Models.Pedido
{
    public class PedidoModel
    {
        public List<ProdutoModel> Produtos { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime Data { get; set; }

    }
}
