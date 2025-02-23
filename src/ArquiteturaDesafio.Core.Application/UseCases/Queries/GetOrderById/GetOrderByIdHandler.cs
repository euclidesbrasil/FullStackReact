using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Queries.GetOrderById
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdRequest, GetOrderByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetOrderByIdHandler(IUnitOfWork unitOfWork,
            IOrderRepository saleRepository,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _saleRepository = saleRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<GetOrderByIdResponse> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
        {

            var sale = await _saleRepository.GetSaleWithItemsAsync(request.id, cancellationToken);

            if (sale is null)
            {
                throw new KeyNotFoundException($"Sale with ID  {request.id} does not exist in our database");
            }

            return _mapper.Map<GetOrderByIdResponse>(sale);
        }
    }
}
