using CestaFeira.Domain.Entityes.Base;

namespace CestaFeira.Domain.Entityes
{
    public class VendaEntity:BaseEntity
    {
        public IEnumerable<ProdutoEntity> Produtos { get; set; }
        public UsuarioEntity Usuario { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
