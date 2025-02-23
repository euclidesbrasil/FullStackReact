namespace ArquiteturaDesafio.Application.UseCases.Commands.Product.CreateProduct;

public class CreateProductResponse 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Image { get; set; }
    public CreateProductRatingResponse Rating { get; set; }
}

public class CreateProductRatingResponse
{
    public double Rate { get; set; }
    public int Count { get; set; }
}