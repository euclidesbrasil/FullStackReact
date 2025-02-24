using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquiteturaDesafio.Core.Domain.ValueObjects;

namespace ArquiteturaDesafio.Core.Domain.Entities
{
    public class OrderRead : BaseEntityNoRelational
    {
        public class OrderReadModel
        {
            public Guid OrderId { get; set; } // ID original do pedido no SQL Server

            public CustumerOrder Customer { get; set; }

            public DateTime OrderDate { get; set; }

            public decimal TotalAmount { get; set; }

            public string Status { get; set; } // Pode ser "Pending", "Completed", etc.

            public List<OrderItemRead> Items { get; set; }
        }
    }
}
