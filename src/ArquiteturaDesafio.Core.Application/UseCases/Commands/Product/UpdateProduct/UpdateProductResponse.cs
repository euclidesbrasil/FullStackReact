﻿namespace ArquiteturaDesafio.Application.UseCases.Commands.Product.UpdateProduct;

public sealed record UpdateProductResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Image { get; set; }
    public UpdateProductRatingResponse Rating { get; set; }
}

public class UpdateProductRatingResponse
{
    public double Rate { get; set; }
    public int Count { get; set; }
}
