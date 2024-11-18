using AutoMapper;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Query.Produto;
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
            CreateMap<ProdutoEntity, ProdutoDto>().ReverseMap();

            CreateMap<ProdutoDto, ProdutoCreateCommand>().ReverseMap();

            CreateMap<ProdutoEntity, ProdutoCreateCommand>().ReverseMap();

            CreateMap<ProdutoEntity, ProdutoQuery>().ReverseMap();


            CreateMap<ProdutoDto, ProdutoUpdateCommad>().ReverseMap();

            CreateMap<ProdutoEntity, ProdutoUpdateCommad>().ReverseMap();

            CreateMap<ProdutoDto, ProdutoCompleteUpdateCommand>().ReverseMap();

            CreateMap<ProdutoEntity, ProdutoCompleteUpdateCommand>().ReverseMap();

            CreateMap<ProdutoDto, ProdutoDeleteCommand>().ReverseMap();

            CreateMap<ProdutoEntity, ProdutoDeleteCommand>().ReverseMap();
        }
    }
}