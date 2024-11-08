using CestaFeira.Domain.Dtos.Produto;
using CestaFeira.Domain.Entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Dtos.Pedido
{
    public class PedidoProdutoDto
    {
        public Guid? ProdutoId { get; set; }

        public Guid PedidoId { get; set; }
        public int Quantidade { get; set; }
        public decimal valorUnitario { get; set; }

        public ProdutoDto Produto { get; set; }

    }
}
