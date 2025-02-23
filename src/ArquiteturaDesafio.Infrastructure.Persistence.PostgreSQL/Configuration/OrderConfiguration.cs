using ArquiteturaDesafio.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Sales"); // Define o nome da tabela

            builder.HasKey(s => s.Id); // Define a chave primária

            builder.Property(s => s.OrderDate)
                .IsRequired();

            builder.Property(s => s.CustomerId)
                .IsRequired();

            builder.Property(s => s.TotalAmount)
                .HasPrecision(18, 2);

           builder.HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
