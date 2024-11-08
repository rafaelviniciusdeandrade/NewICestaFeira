using CestaFeira.Web.Models.Produto;

namespace CestaFeira.Web.Models.Pedido
{
    public class PedidoProdutoModel
    {
        public Guid? ProdutoId { get; set; }    // Identificador do produto
        public string NomeProduto { get; set; } // Nome do produto
        public int Quantidade { get; set; }     // Quantidade do produto no pedido
        public decimal ValorUnitario { get; set; }      // Preço unitário do produto
        public decimal Total => Quantidade * ValorUnitario; // Propriedade calculada para o total (opcional)
        public ProdutoModel Produto { get; set; }
    }

}
