using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Command.Pedido
{
    public class PedidoUpdateCommand : IRequest<bool>
    {
        public Guid IdPedido { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
    }
}
