using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Query.Pedido;
using CestaFeira.Domain.Validator.Base;
using FluentValidation;

namespace CestaFeira.Domain.Validator.Pedido
{
    public class PedidosProdutorQueryValidator : BaseValidator<PedidosProdutorQuery>
    {
        public PedidosProdutorQueryValidator(IDataModuleDBAps dataModuleDBAps)
        {
            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Informe o id do usuário.");
        }
    }
}