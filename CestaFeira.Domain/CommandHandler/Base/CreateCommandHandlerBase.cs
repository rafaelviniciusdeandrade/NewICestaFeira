using AutoMapper;
using CestaFeira.Domain.Command.Base;
using CestaFeira.Domain.Entityes.Base;
using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Interfaces.Repository;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.CommandHandler.Base
{
    public class CreateCommandHandlerBase<RequestCommand, EntityBase>
         : CommandHandlerBase<RequestCommand, CommandBaseResult>, IRequestHandler<RequestCommand, CommandBaseResult>
             where RequestCommand : IRequest<CommandBaseResult>
             where EntityBase : BaseEntity
    {
        public IRepository<EntityBase> Repository;

        public CreateCommandHandlerBase(
            IDataModuleDBAps dataModule,
            IMapper mapper,
            IValidator<RequestCommand> validator,
            IRepository<EntityBase> repository)
        : base(dataModule, mapper, validator)
        {
            Repository = repository;
        }

        public override async Task<CommandBaseResult> Handle(RequestCommand request, CancellationToken cancellationToken)
        {
            await base.Handle(request, cancellationToken);

            var objEntity = mapper.Map<EntityBase>(request);

            var dbEnitty = await Repository.InsertAsync(objEntity);

            return new CommandBaseResult()
            {
                Result = dbEnitty.Id
            };
        }

    }
}
