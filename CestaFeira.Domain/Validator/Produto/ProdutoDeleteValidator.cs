using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Validator.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Validator.Produto
{
    public class ProdutoDeleteValidator : BaseValidator<ProdutoDeleteCommand>
    {
        public ProdutoDeleteValidator()
        { }
    }
}
