using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.Core.Application.UseCases.DTOs
{
    public class OrderBaseDTO
    {
        public DateTime OrderDate { get; set; } // Data da venda
        public Guid CustomerId { get; set; }  // Identidade Externa do Cliente
        public List<OrderItemBaseDTO> Items { get; set; }
    }
}
