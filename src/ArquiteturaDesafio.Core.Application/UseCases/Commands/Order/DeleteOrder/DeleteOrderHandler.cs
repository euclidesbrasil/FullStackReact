using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using ArquiteturaDesafio.Core.Domain.Entities;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.DeleteOrder;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderRequest, DeleteOrderResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public DeleteOrderHandler(IUnitOfWork unitOfWork,
        IOrderRepository repository,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DeleteOrderResponse> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
    {
        ArquiteturaDesafio.Core.Domain.Entities.Order order = await _repository.Get(request.id, cancellationToken);

        if (order == null)
        {
            throw new KeyNotFoundException($"Venda com o ID{request.id} não existe na base de dados.");
        }

        _repository.Delete(order);
        await _unitOfWork.Commit(cancellationToken);
        return new DeleteOrderResponse("Venda removida com sucesso.");
    }
}
