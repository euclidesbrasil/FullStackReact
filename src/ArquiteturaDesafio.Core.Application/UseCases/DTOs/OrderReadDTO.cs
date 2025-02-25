using ArquiteturaDesafio.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class OrderReadDTO
    {
        public string OrderId { get; set; } // ID original do pedido no SQL Server

        public CustomerOrderDTO Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } // Pode ser "Pending", "Completed", etc.

        public List<OrderItemRead> Items { get; set; }
    }
}
