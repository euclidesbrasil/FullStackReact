using MediatR;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetOrdersQuery
{
    public sealed record GetOrdersQueryRequest(int page, int size, string order, Dictionary<string, string> filters = null) : IRequest<GetOrdersQueryResponse>;
}
