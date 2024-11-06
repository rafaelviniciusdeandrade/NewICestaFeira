using AutoMapper;
using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Query.Produto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.QueryHandler.Produto
{
    public class ProdutoCompleteQueryHandler : IRequestHandler<ProdutoCompleteQuery, List<ProdutoDto>>
    {
        private readonly IDataModuleDBAps _dataModule;
        private readonly IMapper _mapper;

        public ProdutoCompleteQueryHandler(IDataModuleDBAps dataModule, IMapper mapper)
        {
            _dataModule = dataModule;
            _mapper = mapper;
        }
        public async Task<List<ProdutoDto>> Handle(ProdutoCompleteQuery request, CancellationToken cancellationToken)
        {
            var listaEntity = _dataModule.ProdutoRepository
                     .ListNoTracking(x => true)
                      .Include(p => p.Usuario)
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