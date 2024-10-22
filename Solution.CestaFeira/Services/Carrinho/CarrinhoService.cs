using CestaFeira.Domain.Command.Pedido;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Entityes;
using CestaFeira.Web.Models.Pedido;
using CestaFeira.Web.Services.Interfaces;
using MediatR;

namespace CestaFeira.Web.Services.Carrinho
{
    public class CarrinhoService : ICarrinhoService
    {
        private readonly IMediator _mediator;

        public CarrinhoService(IMediator mediator)
        {
            _mediator = mediator;
        }

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

        public async Task<bool> CadastrarCarrinho(PedidoModel carrinho)
        {
            var produtos = carrinho.Produtos; // Supondo que carrinho.Produtos seja uma coleção de ProdutoEntity

            var pedidoCommand = new PedidoCreateCommand
            {
                UsuarioId = carrinho.UsuarioId,
                Data = DateTime.Now,
                Produtos = produtos.Select(produto => new ProdutoCreateCommand
                {
                    Id=produto.Id,
                    Nome = produto.Nome,
        valorUnitario=produto.valorUnitario,
        quantidade=produto.Quantidade
        // Mapeie outras propriedades conforme necessário
                }).ToList()
            };

            var result = await _mediator.Send(pedidoCommand);

            if (result.Success)
            {
                var usuarioDto = (result.Result as LoginDtoResult)?.Usuario;
                return true;
            }
            else
            {
                return false;
            }
            return false;
        }
    }
}
