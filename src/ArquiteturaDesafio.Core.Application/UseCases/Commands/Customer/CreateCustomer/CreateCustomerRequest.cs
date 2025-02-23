using ArquiteturaDesafio.Core.Domain.Entities;

using MediatR;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Customer.CreateCustomer
{
    public class CreateCustomerRequest : CustomerDTO, IRequest<CreateCustomerResponse>
    {
    }
}
