using ArquiteturaDesafio.Application.UseCases.Queries.GetOrderById;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Application.UseCases.Queries.GetOrderById
{
    public sealed record GetOrderByIdRequest(Guid id) : IRequest<GetOrderByIdResponse>;
}
