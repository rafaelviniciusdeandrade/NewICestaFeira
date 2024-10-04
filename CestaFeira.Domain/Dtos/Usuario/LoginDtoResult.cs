using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Dtos.Usuario
{
    public class LoginDtoResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenExpiration { get; set; }
        public UsuarioDto Usuario { get; set; }

    }
}