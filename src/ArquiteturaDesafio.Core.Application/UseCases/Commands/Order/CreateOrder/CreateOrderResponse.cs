using ArquiteturaDesafio.Application.UseCases.Commands.Customer.DeleteCustomer;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.CreateSale
{
    public sealed record CreateOrderResponse(Guid id);
}
