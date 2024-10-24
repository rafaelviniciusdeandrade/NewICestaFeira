namespace CestaFeira.Web.Models.Pedido
{
    public class PedidoProdutoRetModel
    {
        public Guid UsuarioId { get; set; }
        public DateTime Data { get; set; }
        public List<PedidoProdutoModel> Produtos { get; set; }
    }
}
