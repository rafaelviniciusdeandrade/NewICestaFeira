using CestaFeira.Domain.Command.Base;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Entityes;
using MediatR;


namespace CestaFeira.Domain.Command.Pedido
{
    public class PedidoCreateCommand : IRequest<CommandBaseResult>
    {
        public List<ProdutoCreateCommand> Produtos { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }

    }
}
