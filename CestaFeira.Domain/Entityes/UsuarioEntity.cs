

using CestaFeira.Domain.Entityes.Base;

namespace CestaFeira.Domain.Entityes
{
    public class UsuarioEntity : BaseEntity
    {
        public string cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Cel { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public DateTime Data { get; set; }
        public bool Ativo { get; set; }
        public string Perfil { get; set; }
        public IEnumerable<ProdutoEntity> Produtos { get; set; }

    }
}
