using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Context;
using Microsoft.EntityFrameworkCore;
using ArquiteturaDesafio.Core.Domain.Common;
using System.Linq;
using ArquiteturaDesafio.Core.Domain.Entities;
using System.Linq.Dynamic.Core;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Extensions;

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<PaginatedResult<Order>> GetSalesPagination(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var query = _context.Orders.Where(x => true);
        query = query.ApplyFilters(paginationQuery.Filter);

        paginationQuery.Order = paginationQuery.Order ?? "id asc";
        query = query.OrderBy(paginationQuery.Order);
        var totalCount = await query.CountAsync(cancellationToken); // Total de itens sem paginação
        var items = await query
            .Skip(paginationQuery.Skip)
            .Take(paginationQuery.Size)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<Order>
        {
            Data = items,
            TotalItems = totalCount,
            CurrentPage = paginationQuery.Page
        };
    }

    public async Task<Order> GetSaleWithItemsAsync(Guid id, CancellationToken cancellationToken)
    {
        var itens =  await _context.Orders
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return itens;
    }

}
