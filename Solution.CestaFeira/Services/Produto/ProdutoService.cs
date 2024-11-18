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
                quantidade = produto.Quantidade,
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
        public async Task<bool> EditarProduto(ProdutoModel produto)
        {
            if (produto.Id != null)
            {
                var prod = await ConsultarProdutosId((Guid)produto.Id);
                var produtoCommand = new ProdutoCompleteUpdateCommand
                {
                    Id=prod.Id,
                    Nome = !string.IsNullOrEmpty(produto.Nome) ? produto.Nome : prod.Nome,
                    Descricao = !string.IsNullOrEmpty(produto.Descricao) ? produto.Descricao : prod.Descricao,
                    quantidade = produto.Quantidade > 0 ? produto.Quantidade : prod.Quantidade,
                    valorUnitario = produto.valorUnitario > 0 ? produto.valorUnitario : prod.valorUnitario,
                    imagem = produto.imagem ?? prod.imagem,
                    UsuarioId = produto.UsuarioId.HasValue && produto.UsuarioId != Guid.Empty ? produto.UsuarioId.Value : prod.UsuarioId.GetValueOrDefault(),
                    Usuario = new UsuarioCreateCommand // Mapeia as propriedades de UsuarioModel para UsuarioCreateCommand
                    {
                        Id=prod.UsuarioId
                    }

                };
                var result = await _mediator.Send(produtoCommand);

                return result;
            }
            else { return false; }

        }
        public async Task<bool> ExcluirProduto(Guid id)
        {
            try
            {
                var produtoDeleteCommand = new ProdutoDeleteCommand
                {
                    Id = id
                };
                var result = await _mediator.Send(produtoDeleteCommand);

                return result;
            }
            catch
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
                    Id = produtoDto.Id,
                    Nome = produtoDto.Nome,
                    Descricao = produtoDto.Descricao,
                    Quantidade = produtoDto.quantidade,
                    valorUnitario = produtoDto.valorUnitario,
                    imagem = produtoDto.imagem
                }).ToList();
            }

            return null;
        }


        public async Task<List<ProdutoModel>> ConsultarTodosProdutos()
        {
            var produtoCommad = new ProdutoCompleteQuery
            {
                quantidade = 1
            };

            var result = await _mediator.Send(produtoCommad);

            if (result.Count > 0)
            {
                return result.Select(produtoDto => new ProdutoModel
                {
                    Id = produtoDto.Id,
                    Nome = produtoDto.Nome,
                    Descricao = produtoDto.Descricao,
                    Quantidade = produtoDto.quantidade,
                    valorUnitario = produtoDto.valorUnitario,
                    imagem = produtoDto.imagem,
                    Usuario = new UsuarioModel
                    {
                        Id = produtoDto.Usuario.Id,
                        NomeFantasia = produtoDto.Usuario.NomeFantasia,
                    }
                }).ToList();
            }

            return null;
        }
        public async Task<ProdutoModel> ConsultarProdutosId(Guid ProdutoId)
        {
            var produtoCommad = new ProdutoIdQuery
            {
                ProdutoId = ProdutoId
            };

            var result = await _mediator.Send(produtoCommad);

            if (result.Descricao != null)
            {

                var produtoModel = new ProdutoModel
                {
                    Nome = result.Nome,
                    Descricao = result.Descricao,
                    Quantidade = result.quantidade,
                    valorUnitario = result.valorUnitario,
                    imagem = result.imagem,
                    Id = result.Id,
                    UsuarioId=result.UsuarioId
                };

                // Retorna uma lista contendo o único produto
                return produtoModel;

            }


            return null;
        }
    }
}