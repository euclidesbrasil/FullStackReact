using Microsoft.EntityFrameworkCore;

namespace ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Context
{
    public class DatabaseInitializer
    {
        private readonly AppDbContext _dbContext;
        public DatabaseInitializer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeMigrationAsync()
        {
            // Aplica as migrations ao banco de dados
            await _dbContext.Database.MigrateAsync();
        }
    }
}
