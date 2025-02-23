using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Product.DeleteProduct;

public sealed class DeleteProductValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.id).NotEmpty();
    }
}
