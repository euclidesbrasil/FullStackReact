using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Customer.CreateCustomer;

public sealed class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Identification).NotNull();
        RuleFor(x => x.Identification.Email).NotEmpty().MinimumLength(3).MaximumLength(255);
        RuleFor(x => x.Identification.Phone).NotEmpty().MinimumLength(3).MaximumLength(10);
    }
}
