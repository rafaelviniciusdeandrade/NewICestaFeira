using AutoMapper;
using CestaFeira.Domain.Command.Pedido;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.CommandHandler.Base;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Interfaces.DataModule;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace CestaFeira.Domain.CommandHandler.Pedido
{
    public class PedidoUpdateCommandHandler
        : UpdateCommandHandlerBase<PedidoUpdateCommand, PedidoEntity>
    {
        public PedidoUpdateCommandHandler(
            IDataModuleDBAps dataModule,
        IMapper mapper,
            IValidator<PedidoUpdateCommand> validator)
        : base(dataModule,
              mapper,
              validator,
              dataModule.PedidoRepository)
        {

            OnRequestRepositoryData += async (PedidoUpdateCommand request) =>
            {
                var dbData = await dataModule.PedidoRepository.DataSet.FirstOrDefaultAsync(x => x.Id.Equals(request.IdPedido))
                ??
                throw new ArgumentException("Pedido não localizado.");

                var dbEntity = mapper.Map<PedidoEntity>(dbData);
                dbEntity.Status = request.Status;

                await Repository.UpdateAsync(dbEntity);

                return dbData;
            };
        }
    }
}