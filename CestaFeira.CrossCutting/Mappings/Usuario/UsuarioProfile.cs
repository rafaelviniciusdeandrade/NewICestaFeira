using AutoMapper;
using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Query.Pedido;
using CestaFeira.Domain.Query.Usuario;

namespace CestaFeira.CrossCutting.Mappings.Usuario
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioEntity, UsuarioDto>().ReverseMap();

            CreateMap<UsuarioDto, LoginUsuarioCommand>().ReverseMap();

            CreateMap<UsuarioEntity, LoginUsuarioCommand>().ReverseMap();

            CreateMap<UsuarioDto, UsuarioCreateCommand>().ReverseMap();

            CreateMap<UsuarioEntity, UsuarioCreateCommand>().ReverseMap();

            CreateMap<UsuarioDto, UsuarioQuery>().ReverseMap();

            CreateMap<UsuarioEntity, UsuarioQuery>().ReverseMap();
            CreateMap<UsuarioEntity, UsuarioDto>();




        }
    }
}
