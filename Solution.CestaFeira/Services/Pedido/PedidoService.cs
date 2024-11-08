using CestaFeira.Domain.Command.Pedido;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Query.Pedido;
using CestaFeira.Web.Models.Pedido;
using CestaFeira.Web.Models.Produto;
using CestaFeira.Web.Models.Usuario;
using CestaFeira.Web.Services.Interfaces;
using MediatR;

namespace CestaFeira.Web.Services.Pedido
{
    public class PedidoService : IPedidoService
    {
        private readonly IMediator _mediator;

        public PedidoService(IMediator mediator)
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

        public async Task<List<PedidoProdutoRetModel>> ConsultarPedidos(Guid UsuarioId)
        {
            var pedidoCommad = new PedidoQuery
            {
                UsuarioId = UsuarioId
            };

            var result = await _mediator.Send(pedidoCommad);

            if (result.Count > 0)
            {
                return result.Select(pedidoDto => new PedidoProdutoRetModel
                {
                    UsuarioId = pedidoDto.UsuarioId,
                    Data = pedidoDto.Data,
                    Status=pedidoDto.Status,
                    Usuario=new UsuarioModel
                    { 
                        Nome=pedidoDto.Usuario.Nome
                    },
                    PedidoProdutos = pedidoDto.ProdutoPedidos.Select(produtoPedidoDto => new PedidoProdutoModel
                    {
                        // Mapeia os dados de cada produto associado ao pedido
                        ProdutoId = produtoPedidoDto.ProdutoId,
                        Quantidade = produtoPedidoDto.Quantidade,
                        ValorUnitario = produtoPedidoDto.valorUnitario,
                        Produto = new ProdutoModel
                        {
                            Nome = produtoPedidoDto.Produto.Nome,
                            valorUnitario=produtoPedidoDto.Produto.valorUnitario
                            // Adicione outras propriedades de ProdutoModel necessárias aqui
                        }
                    }).ToList()

                }).ToList();
            }


            return null;
        }

       
    }
}
