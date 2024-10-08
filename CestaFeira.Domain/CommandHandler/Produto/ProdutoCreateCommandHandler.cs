using AutoMapper;
using CestaFeira.Domain.Command.Base;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.CommandHandler.Base;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Interfaces.DataModule;
using FluentValidation;

namespace CestaFeira.Domain.CommandHandler.Produto
{
    public class ProdutoCreateCommandHandler : CreateCommandHandlerBase<ProdutoCreateCommand, ProdutoEntity>
    {

        public ProdutoCreateCommandHandler(
            IDataModuleDBAps dataModule,
            IMapper mapper,
            IValidator<ProdutoCreateCommand> validator)
        : base(dataModule, mapper, validator, dataModule.ProdutoRepository)
        {
        }

        public override async Task<CommandBaseResult> Handle(ProdutoCreateCommand request, CancellationToken cancellationToken)
        {

            var objEntity = mapper.Map<ProdutoEntity>(request);

            var dbEnitty = await Repository.InsertAsync(objEntity);

            return new CommandBaseResult()
            {
                Result = dbEnitty.Id,
                Success=true
            };
        }

    }
}
