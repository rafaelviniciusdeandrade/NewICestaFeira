using AutoMapper;
using CestaFeira.Domain.Dtos.Pedido;
using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Query.Pedido;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CestaFeira.Domain.QueryHandler.Pedido
{
    public class PedidoQueryHandler : IRequestHandler<PedidoQuery, List<PedidoDto>>
    {
        private readonly IDataModuleDBAps _dataModule;
        private readonly IMapper _mapper;

        public PedidoQueryHandler(IDataModuleDBAps dataModule, IMapper mapper)
        {
            _dataModule = dataModule;
            _mapper = mapper;
        }
        public async Task<List<PedidoDto>> Handle(PedidoQuery request, CancellationToken cancellationToken)
        {
            
            var listaEntity = _dataModule.PedidoRepository
                     .ListNoTracking(x => x.UsuarioId == request.UsuarioId)
                     .Include(p => p.Usuario)
                     .Include(p => p.ProdutoPedidos) // Carrega a lista de produtos associada a cada pedido
                     .ThenInclude(pp => pp.Produto)
                     .ToList();


            if (listaEntity == null)
            {
                return null;
            }

            var listaDto = _mapper.Map<List<PedidoDto>>(listaEntity);

            return listaDto;
        }

    }
}