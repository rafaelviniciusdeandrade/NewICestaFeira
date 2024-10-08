using AutoMapper;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.CrossCutting.Mappings.Produto
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<ProdutoEntity, UsuarioDto>().ReverseMap();

            CreateMap<ProdutoDto, ProdutoCreateCommand>().ReverseMap();

            CreateMap<ProdutoEntity, ProdutoCreateCommand>().ReverseMap();

        }
    }
}