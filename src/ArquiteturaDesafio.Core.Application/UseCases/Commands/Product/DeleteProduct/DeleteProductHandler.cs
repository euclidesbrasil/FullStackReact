using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using Entities = ArquiteturaDesafio.Core.Domain.Entities;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Product.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, DeleteProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public DeleteProductHandler(IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<DeleteProductResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        Entities.Product product = await _productRepository.Get(request.id, cancellationToken);

        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {request.id} does not exist in our database");
        }

        _productRepository.Delete(product);
        await _unitOfWork.Commit(cancellationToken);

        return new DeleteProductResponse("Product deleted with success.");
    }
}
