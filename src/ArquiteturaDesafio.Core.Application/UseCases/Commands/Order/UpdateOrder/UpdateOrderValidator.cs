using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.UpdateSale;

public sealed class UpdateOrderValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderValidator()
    {

        RuleFor(s => s.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");
    }
}
