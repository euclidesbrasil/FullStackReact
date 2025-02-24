using Ambev.Core.Application.UseCases.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.CreateSale
{
    public class CreateOrderRequest:OrderBaseDTO, IRequest<CreateOrderResponse>
    {
    }
}
