using AutoMapper;
using CestaFeira.Domain.Command.Pedido;
using CestaFeira.Domain.Dtos.Pedido;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Query.Pedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.CrossCutting.Mappings.Pedido
{
    public class PedidoProdutoProfile: Profile
    {
        public PedidoProdutoProfile()
        {
            CreateMap<PedidoProdutoEntity, PedidoProdutoDto>().ReverseMap();

            CreateMap<PedidoProdutoDto, PedidoCreateCommand>().ReverseMap();

            CreateMap<PedidoProdutoEntity, PedidoCreateCommand>().ReverseMap();

            CreateMap<PedidoProdutoEntity, PedidoQuery>().ReverseMap();
        }
    }
}
