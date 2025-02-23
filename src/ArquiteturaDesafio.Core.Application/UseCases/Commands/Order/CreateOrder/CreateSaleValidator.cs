using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.CreateSale;

public sealed class CreateSaleValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleValidator()
    {

        RuleFor(s => s.OrderDate)
            .NotEmpty().WithMessage("Order date is required.");

        RuleFor(s => s.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");
    }
}
