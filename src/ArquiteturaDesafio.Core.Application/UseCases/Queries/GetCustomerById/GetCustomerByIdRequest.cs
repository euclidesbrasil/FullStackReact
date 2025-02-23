using AutoMapper;

using MediatR;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetCustomerById;

public sealed record GetCustomerByIdRequest(Guid id) : IRequest<GetCustomerByIdResponse>;