using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Command.Produto
{
    public class ProdutoUpdateCommad : IRequest<bool>
    {
        public Guid? Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int quantidade { get; set; }
        public double valorUnitario { get; set; }
        public byte[] imagem { get; set; }
    }
}
