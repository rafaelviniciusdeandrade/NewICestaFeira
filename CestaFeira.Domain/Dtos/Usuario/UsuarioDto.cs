

namespace CestaFeira.Domain.Dtos.Usuario
{
    public class UsuarioDto
    {
        public Guid? Id { get; set; }
        public string cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string NomeFantasia { get; set; }
        public string Cel { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public DateTime Data { get; set; }
        public bool Ativo { get; set; }
        public string Perfil { get; set; }
    }
}
