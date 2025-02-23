using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Application.UseCases.Queries.GetProductsQuery
{
    public sealed record GetProductsQueryResponse
    {
        public List<ProductDTO> Data { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
