using Ambev.Core.Application.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class SaleDTO: SaleBaseDTO
    {
        public int Id { get; set; }  // Identificador único da venda
        public List<SaleItemDTO> Items { get; set; }
    }
}
