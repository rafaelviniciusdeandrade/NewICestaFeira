using CestaFeira.Domain.Command.Base;
using MediatR;


namespace CestaFeira.Domain.Command.Usuario
{
    public class LoginUsuarioCommand : IRequest<CommandBaseResult>
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}