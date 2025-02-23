using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class SaleWithDetaislsDTO : SaleDTO
    {
        public string CustomerName { get; set; } // Nome do Cliente (desnormalizado)
        public decimal TotalAmount { get; set; }
    }
}
