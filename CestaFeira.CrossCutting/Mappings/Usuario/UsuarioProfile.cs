using AutoMapper;
using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Entityes;

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

        }
    }
}
