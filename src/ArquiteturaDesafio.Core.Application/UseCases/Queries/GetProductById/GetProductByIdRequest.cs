using AutoMapper;

using MediatR;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetProductById;

public sealed record GetProductByIdRequest(Guid id) : IRequest<GetProductByIdResponse>;