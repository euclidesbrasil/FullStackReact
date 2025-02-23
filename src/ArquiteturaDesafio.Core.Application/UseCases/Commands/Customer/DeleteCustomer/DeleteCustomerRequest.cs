using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Customer.DeleteCustomer;

public sealed record DeleteCustomerRequest(Guid id) : IRequest<DeleteCustomerResponse>;