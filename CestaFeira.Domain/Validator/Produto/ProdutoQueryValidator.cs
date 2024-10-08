using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Query.Produto;
using CestaFeira.Domain.Validator.Base;
using FluentValidation;

namespace CestaFeira.Domain.Validator.Produto
{
    public class ProdutoQueryValidator : BaseValidator<ProdutoQuery>
    {
        public ProdutoQueryValidator(IDataModuleDBAps dataModuleDBAps)
        {
            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Informe o id do usuário.");
        }
    }
}