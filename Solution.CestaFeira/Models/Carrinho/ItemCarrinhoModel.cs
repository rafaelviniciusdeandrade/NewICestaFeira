namespace CestaFeira.Web.Models.Carrinho
{
    public class ItemCarrinhoModel
    {
        public Guid? ProdutoId { get; set; }
        public Guid IdUsuario { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public double ValorUnitario { get; set; }
    }
}
