using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Product.CreateProduct;

public class CreateProductHandler :
       IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductHandler(IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<CreateProductResponse> Handle(CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = _mapper.Map<ArquiteturaDesafio.Core.Domain.Entities.Product>(request);

        _productRepository.Create(product);

        await _unitOfWork.Commit(cancellationToken);
        return new CreateProductResponse(product.Id);
    }
}
