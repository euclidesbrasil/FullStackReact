using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Queries.GetProductsQuery
{
    public sealed record GetProductsQueryRequest(int page, int size, string order, Dictionary<string, string> filters = null) : IRequest<GetProductsQueryResponse>;
}
