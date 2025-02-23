using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;

namespace ArquiteturaDesafio.Application.UseCases.Queries.GetProductsQuery
{ 
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQueryRequest, GetProductsQueryResponse>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetProductsQueryResponse> Handle(GetProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var custumers = await _repository.GetPagination(new Core.Domain.Common.PaginationQuery()
            {
                Order = request.order,
                Page = request.page,
                Size = request.size,
                Filter = request.filters
            }, cancellationToken);

            List<ProductDTO> itensReturn = new List<ProductDTO>();
            foreach(var item in custumers.Data)
            {
                itensReturn.Add(_mapper.Map<ProductDTO>(item));
            }

            return new GetProductsQueryResponse()
            {
                Data = itensReturn,
                CurrentPage = custumers.CurrentPage,
                TotalItems = custumers.TotalItems,
                TotalPages = custumers.TotalPages
            };
        }
    }
}
