using ArquiteturaDesafio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Domain.ValueObjects
{
    public class OrderItemRead : ValueObject
    {
        public OrderItemRead(string productId, string productName, int quantity, decimal unitPrice, decimal totalPrice)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
        }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ProductId;
            yield return ProductName;
            yield return Quantity;
            yield return UnitPrice;
            yield return TotalPrice;
        }
    }
}
