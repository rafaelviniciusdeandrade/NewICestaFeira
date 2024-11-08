using AutoMapper;
using CestaFeira.Domain.Command.Base;
using CestaFeira.Domain.Command.Pedido;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.CommandHandler.Base;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Interfaces.DataModule;
using FluentValidation;


namespace CestaFeira.Domain.CommandHandler.Pedido
{
    public class PedidoCreateCommandHandler: CreateCommandHandlerBase<PedidoCreateCommand, PedidoEntity>
    {

        public PedidoCreateCommandHandler(
            IDataModuleDBAps dataModule,
            IMapper mapper,
            IValidator<PedidoCreateCommand> validator)
        : base(dataModule, mapper, validator, dataModule.PedidoRepository)
        {
        }

        public override async Task<CommandBaseResult> Handle(PedidoCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var pedido = new PedidoEntity
                {
                    UsuarioId = request.UsuarioId,
                    Data = request.Data,
                    Status=request.Status="Aberto",
                    ProdutoPedidos = request.Produtos.Select(prod => new PedidoProdutoEntity
                    {
                        ProdutoId = prod.Id,
                        Quantidade=prod.quantidade
                    }).ToList()
                };
                var dbEnitty = await Repository.InsertAsync(pedido);
                return new CommandBaseResult()
                {
                    Result = dbEnitty.Id,
                    Success = true
                };
            }
            catch(Exception ex) {
                var erro=ex.Message;

                return new CommandBaseResult();
            }


        }

    }
}

