using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Context;
using Microsoft.EntityFrameworkCore;
using ArquiteturaDesafio.Core.Domain.Common;
using System.Linq;
using ArquiteturaDesafio.Core.Domain.Entities;
using System.Linq.Dynamic.Core;
using ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Extensions;

namespace ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    private readonly AppDbContext _context;
    public CustomerRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<PaginatedResult<Customer>> GetCustumerPagination(PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var query = _context.Customers.Where(x => true);
        query = query.ApplyFilters(paginationQuery.Filter);

        paginationQuery.Order = paginationQuery.Order ?? "id asc";
        query = query.OrderBy(paginationQuery.Order);
        var totalCount = await query.CountAsync(cancellationToken); // Total de itens sem paginação
        var items = await query
            .Skip(paginationQuery.Skip)
            .Take(paginationQuery.Size)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<Customer>
        {
            Data = items,
            TotalItems = totalCount,
            CurrentPage = paginationQuery.Page
        };
    }
}
