using AutoMapper;
using CestaFeira.Domain.Entityes.Base;
using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Interfaces.Repository;
using FluentValidation;
using MediatR;


namespace CestaFeira.Domain.CommandHandler.Base
{
    public delegate Task<EntityBase> RequestRepositoryData<EntityBase, RequestCommand>(RequestCommand request);

    public class DeleteCommandHandlerBase<RequestCommand, EntityBase>
       : CommandHandlerBase<RequestCommand, bool>, IRequestHandler<RequestCommand, bool>
           where RequestCommand : IRequest<bool>
           where EntityBase : BaseEntity
    {

        public IRepository<EntityBase> Repository;

        public DeleteCommandHandlerBase(
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

            var objEntity = mapper.Map<EntityBase>(request);

            if (OnRequestRepositoryData != null)
            {
                await OnRequestRepositoryData(request);
            }

            await Repository.DeleteAsync(objEntity.Id);

            return true;
        }

    }
}