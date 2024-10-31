using CestaFeira.Web.Models.Usuario;

namespace CestaFeira.Web.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioModel> ValidarUsuario(UsuarioLoginModel login);
        Task<bool> CadastrarUsuario(UsuarioModel usuario);

    }
}
