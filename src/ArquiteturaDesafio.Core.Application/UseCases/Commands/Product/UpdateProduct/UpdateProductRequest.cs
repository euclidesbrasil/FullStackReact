using ArquiteturaDesafio.Core.Domain.Entities;
using MediatR;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Product.UpdateProduct
{
    public class  UpdateProductRequest: ProductDTO, IRequest<UpdateProductResponse>
    {
       public Guid Id { get; internal set; }
       public void SetId(Guid id)
       {
           Id = id;
       }
    }
}
