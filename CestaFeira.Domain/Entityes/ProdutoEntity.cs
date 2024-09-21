using CestaFeira.Domain.Entityes.Base;

namespace CestaFeira.Domain.Entityes
{
    public class ProdutoEntity : BaseEntity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int quantidade { get; set; }
        public double valorUnitario { get; set; }
        public UsuarioEntity Usuario { get; set; }

    }
}
