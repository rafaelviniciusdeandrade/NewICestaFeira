using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Query.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Query.Produto
{
    public class ProdutoCompleteQuery : BaseQuery, IRequest<List<ProdutoDto>>
    {
        public int quantidade { get; set; }

    }
}
