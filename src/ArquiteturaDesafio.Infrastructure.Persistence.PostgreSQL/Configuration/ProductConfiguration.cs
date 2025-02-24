using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ArquiteturaDesafio.Core.Domain.Entities;

namespace CleanArch.Infrastructure.SQLServer.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products"); // Define o nome da tabela

            builder.HasKey(p => p.Id); // Define a chave primária

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .HasPrecision(18, 2); // Define precisão para valores decimais
        }
    }
}
