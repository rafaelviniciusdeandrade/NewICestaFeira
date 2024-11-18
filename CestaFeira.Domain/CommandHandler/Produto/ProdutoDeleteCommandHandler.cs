using AutoMapper;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.CommandHandler.Base;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Interfaces.DataModule;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.CommandHandler.Produto
{
    public class ProdutoDeleteCommandHandler
          : DeleteCommandHandlerBase<ProdutoDeleteCommand, ProdutoEntity>
    {
        public ProdutoDeleteCommandHandler(
            IDataModuleDBAps dataModule,
            IMapper mapper,
            IValidator<ProdutoDeleteCommand> validator)
        : base(dataModule, mapper, validator, dataModule.ProdutoRepository) { }
    }
}
