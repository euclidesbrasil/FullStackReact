using Entities = ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using AutoMapper.Internal;

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

            sale.VerifyIfAllItensAreMine(allRequestItensIds);

            if (customer is null)
            {
                throw new KeyNotFoundException($"Customer with ID {request.CustomerId} does not exist in our database");
            }
          

            sale.Update(request.OrderDate, customer, request.Status);

            List<Guid> idsProducts = request.Items.Select(i => i.ProductId).ToList();
            var productsUsed = await _productRepository.Filter(x => idsProducts.Contains(x.Id), cancellationToken);

            List<Entities.OrderItem> itens = request.Items.Select(x=> new OrderItem(request.Id,x.ProductId,"",x.Quantity,0,x.Id)).ToList();
            var newItems = itens.Where(x => x.Id == Guid.Empty).ToList();
            var editItems = itens.Where(x => x.Id != Guid.Empty).ToList();
            var removeItems = sale.Items.Where(x => !allRequestItensIds.Contains(x.Id)).ToList();
            
            sale.RemoveItems(removeItems.Select(ids=> ids.Id).ToList());
            sale.AddItems(newItems, productsUsed);
            sale.UpdateItems(editItems, productsUsed);

            _saleRepository.Update(sale);
            await _unitOfWork.Commit(cancellationToken);

            OrderRead orderEvent = new OrderRead(
                    sale.Id,
                    new CustomerOrder(customer.Id, customer.Name, customer.Identification.Email, customer.Identification.Phone),
                    sale.OrderDate,
                    sale.TotalAmount,
                    sale.Status.ToString(), sale.Items.Select(i => new OrderItemRead(i.ProductId.ToString(), i.Name, i.Quantity, i.UnitPrice, i.TotalPrice,i.Id)).ToList()
                    );


            await _producerMessage.SendMessage(orderEvent, "order.updated");
            return new UpdateOrderResponse("Order atualizada com sucesso.");
        }
    }
}
