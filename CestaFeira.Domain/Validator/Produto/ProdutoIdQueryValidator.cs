using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Query.Produto;
using CestaFeira.Domain.Validator.Base;
using FluentValidation;

namespace CestaFeira.Domain.Validator.Produto
{
    public class ProdutoIdQueryValidator : BaseValidator<ProdutoIdQuery>
    {
        public ProdutoIdQueryValidator(IDataModuleDBAps dataModuleDBAps)
        {
            RuleFor(x => x.ProdutoId).NotEmpty().WithMessage("Informe o id do produto.");
        }
    }
}
