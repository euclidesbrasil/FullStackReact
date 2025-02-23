using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class SaleItemDTO: SaleItemBaseDTO
    {
        public int Id { get; set; } // Identificador único do item
        public int SaleId { get; set; } // Relacionamento com a venda (FK)
        public string ProductName { get; set; } // Nome do Produto (desnormalizado)

    }
}
