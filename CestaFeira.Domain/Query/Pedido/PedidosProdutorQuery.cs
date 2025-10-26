using CestaFeira.Domain.Dtos.Pedido;
using CestaFeira.Domain.Query.Base;
using MediatR;


namespace CestaFeira.Domain.Query.Pedido
{
    public class PedidosProdutorQuery : BaseQuery, IRequest<List<PedidoDto>>
    {
        public Guid? UsuarioId { get; set; }

    }
}

