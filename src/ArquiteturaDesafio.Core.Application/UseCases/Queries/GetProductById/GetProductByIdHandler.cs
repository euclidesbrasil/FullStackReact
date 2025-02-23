using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using MediatR;
namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetProductById;
public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest query, CancellationToken cancellationToken)
    {
        Product customer = await _repository.Get(query.id, cancellationToken);
        return _mapper.Map<GetProductByIdResponse>(customer);
    }
}
