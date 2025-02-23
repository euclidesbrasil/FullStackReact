using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Product.UpdateProduct;

public sealed class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
    }
}
