
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;

namespace ArquiteturaDesafio.Core.Domain.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<List<Product>> GetProductByListIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
}
