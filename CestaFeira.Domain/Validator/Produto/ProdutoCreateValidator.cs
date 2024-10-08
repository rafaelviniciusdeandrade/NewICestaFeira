using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Validator.Base;
using FluentValidation;


namespace CestaFeira.Domain.Validator.Produto
{
    public class ProdutoCreateValidator : BaseValidator<ProdutoCreateCommand>
    {
        public ProdutoCreateValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("Informe o nome do produto.");
            RuleFor(x => x.Descricao).NotEmpty().WithMessage("Informe a descrição do produto.");
            RuleFor(x => x.quantidade).NotEmpty().WithMessage("Informe a quantidade do produto.");
            RuleFor(x => x.valorUnitario).NotEmpty().WithMessage("Informe o valor unitário do produto.");
        }
    }
}