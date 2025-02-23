using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Queries.GetCustomersQuery
{
    public sealed record GetCustomersQueryRequest(int page, int size, string order, Dictionary<string, string> filters = null) : IRequest<GetCustomersQueryResponse>;
}
