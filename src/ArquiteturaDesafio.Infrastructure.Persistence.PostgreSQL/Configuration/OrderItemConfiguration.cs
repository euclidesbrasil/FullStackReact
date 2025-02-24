using ArquiteturaDesafio.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.OrderId)
                .IsRequired();

            builder.Property(i => i.ProductId)
                .IsRequired();

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");


            builder.Property(i => i.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}
