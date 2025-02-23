using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;

namespace ArquiteturaDesafio.Core.Domain.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<PaginatedResult<Order>> GetSalesPagination(PaginationQuery paginationQuery, CancellationToken cancellationToken);
    Task<Order> GetSaleWithItemsAsync(Guid saleId, CancellationToken cancellationToken);
}
