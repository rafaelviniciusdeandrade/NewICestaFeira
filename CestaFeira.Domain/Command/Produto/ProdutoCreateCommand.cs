using CestaFeira.Domain.Command.Base;
using CestaFeira.Domain.Entityes;
using MediatR;


namespace CestaFeira.Domain.Command.Produto
{
    public class ProdutoCreateCommand : IRequest<CommandBaseResult>
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int quantidade { get; set; }
        public double valorUnitario { get; set; }
        public byte[] imagem { get; set; }
        public Guid UsuarioId { get; set; } 
        public UsuarioEntity Usuario { get; set; }
    }
}