using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Order.DeleteOrder;

public sealed record DeleteOrderRequest(Guid id) : IRequest<DeleteOrderResponse>;