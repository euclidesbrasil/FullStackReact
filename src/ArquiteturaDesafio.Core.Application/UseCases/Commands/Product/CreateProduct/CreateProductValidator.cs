using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Product.CreateProduct;

public sealed class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
    }
}
