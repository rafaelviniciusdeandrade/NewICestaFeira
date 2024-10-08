using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Web.Models.Produto;
using CestaFeira.Web.Services.Interfaces;
using MediatR;
using Nest;

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
                UsuarioId=produto.UsuarioId
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
    }
}