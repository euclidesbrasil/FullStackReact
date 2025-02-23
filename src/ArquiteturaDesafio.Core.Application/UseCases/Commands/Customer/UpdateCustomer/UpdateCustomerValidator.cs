using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Customer.UpdateCustomer;

public sealed class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
