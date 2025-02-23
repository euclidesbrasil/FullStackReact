using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Customer.UpdateCustomer
{
    public class UpdateCustomerRequest : CustomerDTO, IRequest<UpdateCustomerResponse>
    {
        public Guid Id { get; private set; }

        public void SetId(Guid id)
        {
            Id = id;
        }
    }
}
