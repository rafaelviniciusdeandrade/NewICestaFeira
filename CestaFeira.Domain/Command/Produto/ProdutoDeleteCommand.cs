using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Command.Produto
{
    public class ProdutoDeleteCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}

