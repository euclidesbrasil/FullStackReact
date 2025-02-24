using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.DeleteOrder;

public sealed class DeleteOrderValidator : AbstractValidator<DeleteOrderRequest>
{
    public DeleteOrderValidator()
    {
        RuleFor(x => x.id).NotEmpty();
    }
}
