using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Dtos.Usuario;


namespace CestaFeira.Domain.Dtos.Pedido
{
    public class PedidoDto
    {
        public ProdutoDto Produtos { get; set; }
        public UsuarioDto Usuario { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime Data { get; set; }
    }
}
