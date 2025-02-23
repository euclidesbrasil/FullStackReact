using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class SaleItemBaseDTO
    {
        public SaleItemBaseDTO()
        {
        }
        public Guid ProductId { get; set; } // Identidade Externa do Produto
        public int Quantity { get; set; } // Quantidade vendida
        public decimal UnitPrice { get; internal set; } // Preço unitário
        public decimal Discount { get; internal set; } // Valor do desconto aplicado
        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                totalPrice = (UnitPrice * Quantity) - Discount;
                return totalPrice;
            }
        }
    }
}
