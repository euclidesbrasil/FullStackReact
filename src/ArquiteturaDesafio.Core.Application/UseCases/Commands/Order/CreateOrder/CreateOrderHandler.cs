﻿using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using MediatR;

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
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.CreateSale
{

    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _saleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IProducerMessage _producerMessage;
        private List<Entities.Product> _allProducts;
        public CreateOrderHandler(IUnitOfWork unitOfWork,
            IOrderRepository saleRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IMapper mapper,
            IProducerMessage producerMessage)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _producerMessage = producerMessage ?? throw new ArgumentNullException(nameof(producerMessage));
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var sale = _mapper.Map<Entities.Order>(request);

                var allProducts = await _productRepository.GetAll(cancellationToken);
                var customers = await _customerRepository.Filter(x => x.Id == request.CustomerId, cancellationToken);
                if (customers is null)
                {
                    throw new KeyNotFoundException($"Customer with ID {request.CustomerId} does not exist in our database");
                }
                var customer = customers.FirstOrDefault();


                var idsProducts = request.Items.Select(i => i.ProductId).ToList();
                var productsUsed = allProducts.Where(x => idsProducts.Contains(x.Id)).ToList();

                if (productsUsed.Count == 0)
                {
                    throw new KeyNotFoundException($"Invalid products: {string.Join(",", idsProducts)} do not exist in the database.");
                }

                var itens = _mapper.Map<List<Entities.OrderItem>>(request.Items);
                sale.ClearItems();
                sale.AddItems(itens, productsUsed);

                _saleRepository.Create(sale);
                await _unitOfWork.Commit(cancellationToken);
                OrderRead orderEvent = new OrderRead(
                    sale.Id,
                    new CustomerOrder(customer.Id, customer.Name, customer.Identification.Email, customer.Identification.Phone),
                    sale.OrderDate,
                    sale.TotalAmount,
                    sale.Status.ToString(), sale.Items.Select(i => new OrderItemRead(i.ProductId.ToString(), i.Name, i.Quantity, i.UnitPrice, i.TotalPrice, i.Id)).ToList()
                    );


                await _producerMessage.SendMessage(orderEvent, "order.created");

                return new CreateOrderResponse(sale.Id);
            }
            catch (Exception e)
            {
                string erro = e.ToString();
                throw;
            }
        }
    }


}
