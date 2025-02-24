using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Context;

namespace ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    public async Task Commit(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
