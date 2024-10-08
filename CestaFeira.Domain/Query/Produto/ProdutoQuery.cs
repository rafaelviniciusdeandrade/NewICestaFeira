using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Query.Base;
using MediatR;

namespace CestaFeira.Domain.Query.Produto
{
    public class ProdutoQuery : BaseQuery, IRequest<List<ProdutoDto>>
    {
        public Guid? UsuarioId { get; set; }
    }
}
