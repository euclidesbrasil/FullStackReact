using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.CreateSale;

public sealed class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {

        RuleFor(s => s.OrderDate)
            .NotEmpty().WithMessage("Order date is required.");

        RuleFor(s => s.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");
    }
}
