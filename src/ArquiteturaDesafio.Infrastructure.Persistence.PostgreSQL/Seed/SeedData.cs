﻿using Entities = ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquiteturaDesafio.Core.Domain.Interfaces;

namespace ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Seed
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, IJwtTokenService tokenService)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;
                }

                IJwtTokenService _tokenService = tokenService;
                context.Users.AddRange(
                    new Entities.User("user.admin@arquitetura.desafio.com.br", "admin", "s3nh@", "User", "Admin",
                    // public Address(string city, string street, int number, string zipCode, Geolocation geolocation)
                    new Core.Domain.ValueObjects.Address("Fortaleza", "Rua em Fortaleza", 1000, "60000-000", new Core.Domain.ValueObjects.Geolocation("-3.71839", "-38.5434")),
                    "85999999999", Core.Domain.Enum.UserStatus.Active, Core.Domain.Enum.UserRole.Admin, _tokenService)

                );

                context.Products.AddRange(
                    new Entities.Product("Product 1", 100.90M),
                    new Entities.Product("Product 2", 200.10M),
                    new Entities.Product("Product 3", 300.30M)
                );

                context.Customers.AddRange(
                    new Entities.Customer("Cliente 1", new Core.Domain.ValueObjects.InfoContact("email1@email.com", "988909090")),
                    new Entities.Customer("Cliente 2", new Core.Domain.ValueObjects.InfoContact("email2@email.com", "988907777")),
                    new Entities.Customer("Cliente 3", new Core.Domain.ValueObjects.InfoContact("email2@email.com", "988900000"))
                );
                context.SaveChanges();
            }
        }
    }
}
