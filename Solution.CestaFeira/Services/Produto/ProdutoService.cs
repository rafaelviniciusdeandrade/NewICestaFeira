using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Query.Produto;
using CestaFeira.Web.Models.Produto;
using CestaFeira.Web.Models.Usuario;
using CestaFeira.Web.Services.Interfaces;
using MediatR;

namespace CestaFeira.Web.Services.Produto
{
    public class ProdutoService : IProdutoService
    {
        private readonly IMediator _mediator;

        public ProdutoService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> CadastrarProduto(ProdutoModel produto)
        {
            var produtoCommand = new ProdutoCreateCommand
            {
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                quantidade = produto.quantidade,
                valorUnitario = produto.valorUnitario,
                imagem = produto.imagem,
                UsuarioId = produto.UsuarioId
            };

            var result = await _mediator.Send(produtoCommand);

            if (result.Success)
            {
                var usuarioDto = (result.Result as LoginDtoResult)?.Usuario;
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<ProdutoModel>> ConsultarProdutos(Guid UsuarioId)
        {
            var produtoCommad = new ProdutoQuery
            {
                UsuarioId = UsuarioId
            };

            var result = await _mediator.Send(produtoCommad);

            if (result.Count > 0)
            {
                return result.Select(produtoDto => new ProdutoModel
                {
                    Nome = produtoDto.Nome,
                    Descricao = produtoDto.Descricao,
                    quantidade = produtoDto.quantidade,
                    valorUnitario=produtoDto.valorUnitario,
                    imagem = produtoDto.imagem
                }).ToList();
            }

            return null;
        }
    }
}