using ArquiteturaDesafio.Infrastructure.CrossCutting.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.General.Api.Filters;
using Microsoft.OpenApi.Models;
using ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Configuration;
using MongoDB.Driver;
using ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Context;
using Microsoft.EntityFrameworkCore;
using ArquiteturaDesafio.Infrastructure.Persistence.SQLServer.Seed;
using ArquiteturaDesafio.Core.Domain.Interfaces;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "General Api - Controle de Pedidos com o banco SQLSEVER e MONGODB", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Informe o token. Examplo: \"12345abcdef\""
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        var urlsAccess = builder.Configuration.GetSection("UrlAccess:Values").Get<string[]>();
        builder.Services.AddCors(o => o.AddPolicy("CORSPolicy", builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        }));

        // Registro dos servi�os
        builder.Services.AddInfrastructure(builder.Configuration);

        // Adicione o servi�o de autentica��o JWT
        var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });


        builder.Logging.AddFile("Logs/app_log.txt"); // Log em arquivo
        var app = builder.Build();

        // Registra o filtro de exce��o personalizado
        app.UseMiddleware<ExceptionHandlingMiddleware>(app.Environment.IsDevelopment());
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.UseCors("CORSPolicy");

        // Instanciar banco MongoDB
        using (var scope = app.Services.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            var _jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
            var initializer = new MongoDbInitializer(database);
            await initializer.InitializeAsync();

            try
            {
                using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                    context.Database.Migrate();
                    Console.WriteLine("Migration foi executado");

                    SeedData.Initialize(serviceScope.ServiceProvider, _jwtTokenService);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        await app.RunAsync();
    }
}