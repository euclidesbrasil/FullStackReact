using Entities = ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.UpdateSale
{
    public class UpdateOrderHandler :
       IRequestHandler<UpdateOrderRequest, UpdateOrderResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _saleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IProducerMessage _producerMessage;
        public UpdateOrderHandler(IUnitOfWork unitOfWork,
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

        public async Task<UpdateOrderResponse> Handle(UpdateOrderRequest request,
            CancellationToken cancellationToken)
        {
            var allRequestItensIds = request.Items.Select(x => x.Id).ToList();
            var sale = await _saleRepository.GetSaleWithItemsAsync(request.Id, cancellationToken);
            var customer = await _customerRepository.Get(request.CustomerId, cancellationToken);
            var saleToUpdate = _mapper.Map<Entities.Order>(request);

            sale.VerifyIfAllItensAreMine(allRequestItensIds);

            if (customer is null)
            {
                throw new KeyNotFoundException($"Customer with ID {request.CustomerId} does not exist in our database");
            }
          

            sale.Update(saleToUpdate, customer);

            List<Guid> idsProducts = request.Items.Select(i => i.ProductId).ToList();
            var productsUsed = await _productRepository.Filter(x => idsProducts.Contains(x.Id), cancellationToken);

            List<Entities.OrderItem> itens =  _mapper.Map<List<Entities.OrderItem>>(request.Items);
            var newItems = itens.Where(x => x.Id == Guid.Empty).ToList();
            var editItems = itens.Where(x => x.Id != Guid.Empty).ToList();

            sale.AddItems(newItems, productsUsed);
            sale.UpdateItems(editItems, productsUsed);

            _saleRepository.Update(sale);
            await _unitOfWork.Commit(cancellationToken);
            
            // var modifiedSaleEvent = sale.GetSaleModifiedEvent();
            // await _producerMessage.SendMessage(modifiedSaleEvent, "sale.modified");

            return _mapper.Map<UpdateOrderResponse>(sale);
        }
    }
}
