using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Entityes
{
    public class PedidoProdutoEntity
    {
        public Guid? ProdutoId { get; set; }  // Chave estrangeira para Produto
        public ProdutoEntity Produto { get; set; }

        public Guid PedidoId { get; set; }   // Chave estrangeira para Pedido
        public PedidoEntity Pedido { get; set; }

        public int Quantidade { get; set; } // Exemplo de outra propriedade, se necessário
    }
}
