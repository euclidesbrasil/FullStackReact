using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using ArquiteturaDesafio.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers"); // Define o nome da tabela

            builder.HasKey(s => s.Id); // Define a chave primária

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);


            builder.OwnsOne(t => t.Identification, ident =>
            {
                ident.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Email");

                ident.Property(a => a.Phone)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("Phone");
            });
        }
    }
}
