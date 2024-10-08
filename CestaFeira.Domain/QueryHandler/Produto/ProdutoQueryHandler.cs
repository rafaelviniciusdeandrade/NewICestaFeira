using AutoMapper;
using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Entityes;
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
    public class ProdutoQueryHandler : IRequestHandler<ProdutoQuery, List<ProdutoDto>>
    {
        private readonly IDataModuleDBAps _dataModule;
        private readonly IMapper _mapper;

        public ProdutoQueryHandler(IDataModuleDBAps dataModule, IMapper mapper)
        {
            _dataModule = dataModule;
            _mapper = mapper;
        }
        public async Task<List<ProdutoDto>> Handle(ProdutoQuery request, CancellationToken cancellationToken)
        {
            var listaEntity = _dataModule.ProdutoRepository
                                        .ListNoTracking(x => x.UsuarioId == request.UsuarioId)
                                        .ToList();

            if (listaEntity == null)
            {
                return null;
            }

            var listaDto = _mapper.Map<List<ProdutoDto>>(listaEntity);

            return listaDto;
        }

    }
}