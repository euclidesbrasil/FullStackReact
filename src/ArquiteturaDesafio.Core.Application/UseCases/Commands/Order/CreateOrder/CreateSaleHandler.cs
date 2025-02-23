using Entities = ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.CreateSale
{
    public class CreateSaleHandler :
       IRequestHandler<CreateSaleRequest, CreateSaleResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _saleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IProducerMessage _producerMessage;
        public CreateSaleHandler(IUnitOfWork unitOfWork,
            IOrderRepository saleRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IMapper mapper,
            IProducerMessage producerMessage
            )
        {
            _unitOfWork = unitOfWork;
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _producerMessage = producerMessage;
        }

        public async Task<CreateSaleResponse> Handle(CreateSaleRequest request,
            CancellationToken cancellationToken)
        {
            var sale = _mapper.Map<Entities.Order>(request);
            var customer = await _customerRepository.Get(request.CustomerId, cancellationToken);
            if(customer is null)
            {
                throw new KeyNotFoundException($"Customer with ID {request.CustomerId} does not exist in our database");
            }

            List<Guid> idsProducts = request.Items.Select(i => i.ProductId).ToList();
            var productsUsed = await _productRepository.Filter(x => idsProducts.Contains(x.Id), cancellationToken);
            List<Entities.OrderItem> itens = _mapper.Map<List<Entities.OrderItem>>(request.Items);
            sale.ClearItems();
            sale.AddItems(itens, productsUsed);
            _saleRepository.Create(sale);
            await _unitOfWork.Commit(cancellationToken);
            // var createdSaleEvent = sale.GetSaleCreatedEvent();
            // await _producerMessage.SendMessage(createdSaleEvent, "sale.created");

            return _mapper.Map<CreateSaleResponse>(sale);
        }
    }
}
