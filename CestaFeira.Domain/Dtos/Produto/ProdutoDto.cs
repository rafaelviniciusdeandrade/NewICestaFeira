using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Entityes;


namespace CestaFeira.Domain.Dtos.Produto
{
    public class ProdutoDto
    {
        public Guid? Id { get; set; }
        public Guid? UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int quantidade { get; set; }
        public double valorUnitario { get; set; }
        public byte[] imagem { get; set; }
        public UsuarioDto Usuario { get; set; }
    }
}
