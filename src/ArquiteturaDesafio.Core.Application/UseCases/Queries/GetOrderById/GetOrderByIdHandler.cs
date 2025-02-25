using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Queries.GetOrderById
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdRequest, GetOrderByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderReadRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetOrderByIdHandler(IUnitOfWork unitOfWork,
            IOrderReadRepository saleRepository,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _saleRepository = saleRepository;
            _mapper = mapper;
        }
        public async Task<GetOrderByIdResponse> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
        {

            string filtro = request.id.ToString();
            List<OrderRead> sale = await _saleRepository.Filter(x=>x.OrderId == filtro, cancellationToken);

            if (sale is null || sale.Count == 0)
            {
                throw new KeyNotFoundException($"Sale com ID  {request.id} não foi localizado na base de dados.");
            }
            var saleReturn = sale.FirstOrDefault();
            return _mapper.Map<GetOrderByIdResponse>(saleReturn);
        }
    }
}
