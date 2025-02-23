
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;

namespace ArquiteturaDesafio.Core.Domain.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<PaginatedResult<Customer>> GetCustumerPagination(PaginationQuery paginationQuery, CancellationToken cancellationToken);
}
