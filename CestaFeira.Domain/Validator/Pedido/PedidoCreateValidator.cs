using CestaFeira.Domain.Command.Pedido;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Validator.Base;
using FluentValidation;
using System;

namespace CestaFeira.Domain.Validator.Pedido
{
    public class PedidoCreateValidator : BaseValidator<PedidoCreateCommand>
    {
        public PedidoCreateValidator()
        {
            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Informe o ID do Usuário.");
 
        }
    }
}