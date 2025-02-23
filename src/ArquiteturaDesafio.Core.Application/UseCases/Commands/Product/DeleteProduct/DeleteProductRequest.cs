using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Product.DeleteProduct;

public sealed record DeleteProductRequest(Guid id) : IRequest<DeleteProductResponse>;
