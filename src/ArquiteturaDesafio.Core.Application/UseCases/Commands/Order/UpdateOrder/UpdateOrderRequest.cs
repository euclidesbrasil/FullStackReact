using Ambev.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
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
        public void SetId(Guid id)
        {
            Id = id;
        }
    }

    public class UpdateSaleItemRequest : SaleItemBaseDTO
    {
        public Guid Id { get; set; } // Identificador único do item
        public Guid OrderId { get; internal set; } // Relacionamento com a venda (FK)
        public Guid ProductId { get; internal set; } // Relacionamento com a venda (FK)
        public bool IsCancelled { get; set; } // Override para cancelamento do item

    }
}
