using AutoMapper;
using CestaFeira.Domain.Entityes.Base;
using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Interfaces.Repository;
using FluentValidation;
using MediatR;


namespace CestaFeira.Domain.CommandHandler.Base
{
    public class UpdateCommandHandlerBase<RequestCommand, EntityBase>
        : CommandHandlerBase<RequestCommand, bool>, IRequestHandler<RequestCommand, bool>
            where RequestCommand : IRequest<bool>
            where EntityBase : BaseEntity
    {
        public IRepository<EntityBase> Repository;

        public UpdateCommandHandlerBase(
            IDataModuleDBAps dataModule,
            IMapper mapper,
            IValidator<RequestCommand> validator,
            IRepository<EntityBase> repository)
        : base(dataModule, mapper, validator)
        {
            Repository = repository;
        }

        public RequestRepositoryData<EntityBase, RequestCommand> OnRequestRepositoryData { get; set; }

        public override async Task<bool> Handle(RequestCommand request, CancellationToken cancellationToken)
        {
            await base.Handle(request, cancellationToken);

            await OnRequestRepositoryData(request);

            return true;
        }

    }
}
