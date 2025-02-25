using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.UpdateSale
{
    public class UpdateOrderRequest : SaleDTO, IRequest<UpdateOrderResponse>
    {
        public Guid Id { get;  internal set; }
        public List<UpdateSaleItemRequest> Items { get; set; }
        public Guid CustomerId { get; set; }
        public OrderStatus Status { get; set; }
        public void SetId(Guid id)
        {
            Id = id;
        }
    }

    public class UpdateSaleItemRequest 
    {
        public Guid Id { get; set; } // Identificador único do item
        public Guid ProductId { get; set; } // Relacionamento com a venda (FK)
        public int Quantity { get; set; } // Override para cancelamento do item

    }
}
