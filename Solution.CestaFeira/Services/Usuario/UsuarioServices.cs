using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Web.Models.Usuario;
using CestaFeira.Web.Services.Interfaces;
using MediatR;
namespace CestaFeira.Web.Services.Usuario
{

    public class UsuarioServices : IUsuarioService
    {
        private readonly IMediator _mediator;

        public UsuarioServices(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<UsuarioModel> ValidarUsuario(UsuarioLoginModel login)
        {
            var loginCommand = new LoginUsuarioCommand
            {
                Email = login.Email,
                Senha = login.Senha
            };

            var result = await _mediator.Send(loginCommand);

            if (result.Success)
            {
                var usuarioDto = (result.Result as LoginDtoResult)?.Usuario;
                return new UsuarioModel
                {
                    Email = usuarioDto.Email,
                    Perfil = usuarioDto.Perfil,
                    Id = usuarioDto.Id,
                };
            }

            return null;
        }

        public async Task<bool> CadastrarUsuario(UsuarioModel usuario)
        {
            var usuarioCommand = new UsuarioCreateCommand
            {
                cpf = usuario.cpf,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Nome = usuario.Nome,
                NomeFantasia=usuario.NomeFantasia,
                Cel = usuario.cpf,
                Rua = usuario.Rua,
                Numero = usuario.Numero,
                Bairro = usuario.Bairro,
                Cidade = usuario.Cidade,
                Uf=usuario.Uf,
                Data = usuario.Data=DateTime.Now,
                Ativo = usuario.Ativo = true,
                Perfil= usuario.Perfil
            };

            var result = await _mediator.Send(usuarioCommand);

            if (result.Success)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }
}