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

namespace ArquiteturaDesafio.Application.UseCases.Queries.GetCustomersQuery
{ 
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQueryRequest, GetCustomersQueryResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<GetCustomersQueryResponse> Handle(GetCustomersQueryRequest request, CancellationToken cancellationToken)
        {
            var custumers = await _customerRepository.GetCustumerPagination(new Core.Domain.Common.PaginationQuery()
            {
                Order = request.order,
                Page = request.page,
                Size = request.size,
                Filter = request.filters
            }, cancellationToken);

            List<CustomerDTO> itensReturn = new List<CustomerDTO>();
            foreach(var item in custumers.Data)
            {
                itensReturn.Add(_mapper.Map<CustomerDTO>(item));
            }

            return new GetCustomersQueryResponse()
            {
                Data = itensReturn,
                CurrentPage = custumers.CurrentPage,
                TotalItems = custumers.TotalItems,
                TotalPages = custumers.TotalPages
            };
        }
    }
}
