using CestaFeira.Domain.Command.Base;
using CestaFeira.Domain.Entityes;
using CestaFeira.Web.Models.Usuario;
using MediatR;

namespace CestaFeira.Web.Models.Produto
{
    public class ProdutoModel : IRequest<CommandBaseResult>
    {
        public Guid? Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int  Quantidade { get; set; }
        public double valorUnitario { get; set; }
        public byte[] imagem { get; set; }
        public Guid? UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }

    }
}
