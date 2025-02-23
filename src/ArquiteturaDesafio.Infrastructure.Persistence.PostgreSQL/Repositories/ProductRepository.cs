using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Context;
using Microsoft.EntityFrameworkCore;
using ArquiteturaDesafio.Core.Domain.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Extensions;

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<List<Product>> GetProductByListIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }
}
