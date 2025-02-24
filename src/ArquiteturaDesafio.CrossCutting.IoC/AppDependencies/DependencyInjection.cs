using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Context;
using ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Reflection;
using ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Configuration;
using ArquiteturaDesafio.Infrastructure.Persistence.Repositories;
using ArquiteturaDesafio.Core.Application.UseCases.Mapper;
using ArquiteturaDesafio.Core.Application.Shared.Behavior;
using ArquiteturaDesafio.Infrastructure.Security;
using ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ;
using ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ.Consumer;
namespace ArquiteturaDesafio.Infrastructure.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // SQL Server

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")),
                    ServiceLifetime.Scoped
                );

            // MongoDB
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

            // Registra o cliente do MongoDB
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            // Registra o banco de dados do MongoDB
            services.AddScoped<IMongoDatabase>(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return client.GetDatabase(settings.DatabaseName);
            });

            // Adicionar a configuração RabbitMQ

            services.AddScoped<IProducerMessage>(_ => new RabbitMQProducer(configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>().Hostname));

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<JwtTokenService>();
            services.AddScoped<DatabaseInitializer>();
            
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddAutoMapper(typeof(CommonMapper));

            var myhandlers = AppDomain.CurrentDomain.Load("ArquiteturaDesafio.Core.Application");
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(myhandlers);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(Assembly.Load("ArquiteturaDesafio.Core.Application"));

            return services;


        }
    }
}
