using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;
using System.Linq.Expressions;

namespace ArquiteturaDesafio.Core.Domain.Interfaces;

public interface IOrderReadRepository: IBaseRepositoryNoRelational<OrderRead>
{
    Task<PaginatedResult<OrderRead>> GetPaginatedResultAsync(
    Expression<Func<OrderRead, bool>> filter,
    PaginationQuery paginationQuery,
    CancellationToken cancellationToken);
}
