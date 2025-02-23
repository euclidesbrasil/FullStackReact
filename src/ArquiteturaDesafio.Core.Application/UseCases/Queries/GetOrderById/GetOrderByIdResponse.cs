using Ambev.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Application.UseCases.Queries.GetOrderById
{
    public class GetOrderByIdResponse : SaleWithDetaislsDTO
    {
        public decimal TotalAmount
        {
            get
            {
                return Items?.Sum(item => item.TotalPrice) ?? 0;
            }
        }
    }
}
