using AutoMapper;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;

public sealed record DeleteUserRequest(Guid id) : IRequest<DeleteUserResponse>;