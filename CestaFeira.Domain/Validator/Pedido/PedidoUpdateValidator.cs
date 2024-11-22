using CestaFeira.Domain.Command.Pedido;
using CestaFeira.Domain.Validator.Base;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Validator.Pedido
{
    public class PedidoUpdateValidator : BaseValidator<PedidoUpdateCommand>
    {
        public PedidoUpdateValidator()
        {
            RuleFor(x => x.IdPedido).NotEmpty().WithMessage("Informe o ID do Pedido.");

        }
    }
}
