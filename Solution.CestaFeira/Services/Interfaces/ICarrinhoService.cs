using CestaFeira.Web.Models.Pedido;

namespace CestaFeira.Web.Services.Interfaces
{
    public interface ICarrinhoService
    {
        void AdicionarProduto(int produtoId);
        int ObterQuantidadeTotal();
        Task<bool> CadastrarCarrinho(PedidoModel carrinho);
    }
}
