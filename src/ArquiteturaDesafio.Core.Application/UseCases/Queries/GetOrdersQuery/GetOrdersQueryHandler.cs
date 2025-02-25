using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetOrdersQuery
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQueryRequest, GetOrdersQueryResponse>
    {
        private readonly IOrderReadRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(IOrderReadRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<GetOrdersQueryResponse> Handle(GetOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetPaginatedResultAsync(x => true, new Domain.Common.PaginationQuery()
            {
                Order = request.order,
                Page = request.page,
                Size = request.size,
                Filter = request.filters
            }, cancellationToken);

            List<SaleWithDetaislsDTO> itensReturn = new List<SaleWithDetaislsDTO>();
            foreach (var item in sales.Data)
            {
                itensReturn.Add(_mapper.Map<SaleWithDetaislsDTO>(item));
            }

            return new GetOrdersQueryResponse()
            {
                Data = itensReturn,
                CurrentPage = sales.CurrentPage,
                TotalItems = sales.TotalItems,
                TotalPages = sales.TotalPages
            };
        }
    }
}
