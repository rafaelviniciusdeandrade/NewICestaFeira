using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Dtos.Usuario;


namespace CestaFeira.Domain.Dtos.Pedido
{
    public class PedidoDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime Data { get; set; }
        public List<PedidoProdutoDto> ProdutoPedidos { get; set; }
        public UsuarioDto Usuario { get; set; }
        public string Status { get; set; }
    }
}
