using CestaFeira.Domain.Dtos.Pedido;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Query.Base;
using MediatR;


namespace CestaFeira.Domain.Query.Usuario
{
    public class UsuarioQuery : BaseQuery, IRequest<UsuarioDto>
    {
        public Guid? UsuarioId { get; set; }
    }
}

