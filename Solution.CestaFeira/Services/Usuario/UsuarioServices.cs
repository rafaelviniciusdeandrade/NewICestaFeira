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
                    Perfil=usuarioDto.Perfil
                };
            }

            return null;
        }
    }
}