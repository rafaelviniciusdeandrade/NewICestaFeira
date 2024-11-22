using AutoMapper;
using CestaFeira.Domain.Command.Pedido;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Dtos.Pedido;
using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Query.Pedido;

namespace CestaFeira.CrossCutting.Mappings.Pedido
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<PedidoEntity, PedidoProdutoDto>().ReverseMap();

            CreateMap<PedidoEntity, PedidoDto>().ReverseMap();

            CreateMap<PedidoDto, PedidoCreateCommand>().ReverseMap();

            CreateMap<PedidoEntity, PedidoCreateCommand>().ReverseMap();

            CreateMap<PedidoEntity, PedidoQuery>().ReverseMap();

            CreateMap<PedidoDto, PedidoUpdateCommand>().ReverseMap();

            CreateMap<PedidoEntity, PedidoUpdateCommand>().ReverseMap();
        }
    }
}