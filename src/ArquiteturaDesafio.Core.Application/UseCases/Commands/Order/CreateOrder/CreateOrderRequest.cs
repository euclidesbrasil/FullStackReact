using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.CreateSale
{
    public class CreateOrderRequest:OrderBaseDTO, IRequest<CreateOrderResponse>
    {
    }
}
