using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Configuration;
using CleanArch.Infrastructure.SQLServer.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderItem> OrderItems { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}

