using ArquiteturaDesafio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Domain.Entities
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(Guid saleId, Guid productId,string title, int totalQuantity, decimal price)
        {
            OrderId = saleId;
            ProductId = productId;
            Name = title;
            Quantity = totalQuantity;
            UnitPrice = price;
        }

        public Guid OrderId { get; set; } // Relacionamento com a venda (FK)
        public Guid ProductId { get; set; } // Identidade Externa do Produto
        public string Name { get; set; } // Nome do Produto (desnormalizado)
        public int Quantity { get; set; } // Quantidade vendida
        public decimal UnitPrice { get; set; } // Preço unitário
        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                totalPrice = (UnitPrice * Quantity);
                return totalPrice;
            }
            private set { /* Necessário para o Entity Framework */ }

        } // Valor total do item (considerando desconto)
       
    }
}
