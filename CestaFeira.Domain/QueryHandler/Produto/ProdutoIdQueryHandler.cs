using AutoMapper;
using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Query.Produto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.QueryHandler.Produto
{
    public class ProdutoIdQueryHandler : IRequestHandler<ProdutoIdQuery, ProdutoDto>
    {
        private readonly IDataModuleDBAps _dataModule;
        private readonly IMapper _mapper;

        public ProdutoIdQueryHandler(IDataModuleDBAps dataModule, IMapper mapper)
        {
            _dataModule = dataModule;
            _mapper = mapper;
        }
        public async Task<ProdutoDto> Handle(ProdutoIdQuery request, CancellationToken cancellationToken)
        {
            var listaEntity = _dataModule.ProdutoRepository
                                        .ListNoTracking(x => x.Id == request.ProdutoId)
                                        .FirstOrDefault();

            if (listaEntity == null)
            {
                return null;
            }

            var listaDto = _mapper.Map<ProdutoDto>(listaEntity);

            return listaDto;
        }

    }
}