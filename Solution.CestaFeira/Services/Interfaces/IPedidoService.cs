using CestaFeira.Web.Models.Pedido;

namespace CestaFeira.Web.Services.Interfaces
{
    public interface IPedidoService
    {
        void AdicionarProduto(int produtoId);
        int ObterQuantidadeTotal();
        Task<bool> CadastrarCarrinho(PedidoModel carrinho);
        Task<List<PedidoProdutoRetModel>> ConsultarPedidos(Guid UsuarioId);

    }
}
