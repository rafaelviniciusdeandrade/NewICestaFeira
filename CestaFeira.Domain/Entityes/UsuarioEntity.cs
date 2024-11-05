

using CestaFeira.Domain.Entityes.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CestaFeira.Domain.Entityes
{
    public class UsuarioEntity : BaseEntity
    {

        [Column("cpf")]
        public string cpf { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Senha")]
        public string Senha { get; set; }
        [Column("Nome")]
        public string Nome { get; set; }
        [Column("NomeFantasia")]

        public string NomeFantasia { get; set; }
        [Column("Cel")]
        public string Cel { get; set; }
        [Column("Rua")]
        public string Rua { get; set; }
        [Column("Numero")]
        public int Numero { get; set; }
        [Column("Bairro")]
        public string Bairro { get; set; }
        [Column("Cidade")]
        public string Cidade { get; set; }
        [Column("Uf")]
        public string Uf { get; set; }
        [Column("Data")]
        public DateTime Data { get; set; }
        [Column("Ativo")]
        public bool Ativo { get; set; }
        [Column("Perfil")]
        public string Perfil { get; set; }
        public IEnumerable<ProdutoEntity> Produtos { get; set; }
        public IEnumerable<PedidoEntity> Pedidos { get; set; }


    }
}
