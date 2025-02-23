using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using Entities = ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Product.UpdateProduct;

public class UpdateProductHandler :
       IRequestHandler<UpdateProductRequest, UpdateProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductHandler(IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<UpdateProductResponse> Handle(UpdateProductRequest request,
        CancellationToken cancellationToken)
    {

        Entities.Product product = await _productRepository.Get(request.Id, cancellationToken);
        product.Update(request.Name, request.Price);
        _productRepository.Update(product);

        await _unitOfWork.Commit(cancellationToken);
        return new UpdateProductResponse("Produto atualizado com sucesso");
    }
}
