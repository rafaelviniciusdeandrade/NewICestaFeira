using CestaFeira.Web.Services.Interfaces;

namespace CestaFeira.Web.Services.Carrinho
{
    public class CarrinhoService : ICarrinhoService
    {
        private readonly List<int> _produtosNoCarrinho = new List<int>();

        public void AdicionarProduto(int produtoId)
        {
            if (!_produtosNoCarrinho.Contains(produtoId))
            {
                _produtosNoCarrinho.Add(produtoId);
            }
        }
        public int ObterQuantidadeTotal()
        {
            return _produtosNoCarrinho.Count;
        }
    }
}