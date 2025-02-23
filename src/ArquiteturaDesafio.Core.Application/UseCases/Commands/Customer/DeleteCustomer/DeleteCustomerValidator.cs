using ArquiteturaDesafio.Application.UseCases.Commands.Customer.DeleteCustomer;
using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Customer.DeleteCustomer;

public sealed class DeleteCustomerValidator : AbstractValidator<DeleteCustomerRequest>
{
    public DeleteCustomerValidator()
    {
        RuleFor(x => x.id).NotEmpty();
    }
}
