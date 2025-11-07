using AutoMapper;
using CestaFeira.Domain.Dtos.Pedido;
using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Query.Pedido;
using CestaFeira.Domain.Query.Produto;
using CestaFeira.Domain.Query.Usuario;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.QueryHandler.Usuario
{
    public class UsuarioQueryHandler : IRequestHandler<UsuarioQuery, UsuarioDto>
    {
        private readonly IDataModuleDBAps _dataModule;
        private readonly IMapper _mapper;

        public UsuarioQueryHandler(IDataModuleDBAps dataModule, IMapper mapper)
        {
            _dataModule = dataModule;
            _mapper = mapper;
        }
        public async Task<UsuarioDto> Handle(UsuarioQuery request, CancellationToken cancellationToken)
        {
            var listaEntity = _dataModule.UsuarioRepository
                                        .ListNoTracking(x => x.Id == request.UsuarioId)
                                        .FirstOrDefault();

            if (listaEntity == null)
            {
                return null;
            }

            var listaDto = _mapper.Map<UsuarioDto>(listaEntity);

            return listaDto;
        }

    }
}