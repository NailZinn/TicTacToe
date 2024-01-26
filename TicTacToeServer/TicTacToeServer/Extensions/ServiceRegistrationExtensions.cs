using System.Text;
using DataAccess;
using Domain;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Shared.Options;
using TicTacToeServer.Consumers;

namespace TicTacToeServer.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddAppDbContext<TAppDbContext>(this IServiceCollection services, string? connectionString)
        where TAppDbContext : DbContext
    {
        services
            .AddDbContext<DbContext, TAppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            })
            .AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.AddScoped<DbContext, ApplicationDbContext>();
        
        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services, string? connectionString)
    {
        services
            .AddMassTransit(configurator =>
            {
                configurator.AddConsumer<UpdateRatingConsumer>();
                
                configurator.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(connectionString);
                    cfg.ConfigureEndpoints(ctx);
                });
            });

        return services;
    }

    public static IServiceCollection AddMongoDb(this IServiceCollection services, string? connectionString)
    {
        services
            .AddScoped<IMongoDatabase>(_ =>
            {
                var mongoClient = new MongoClient(connectionString);
                return mongoClient.GetDatabase("main");
            });
        
        return services;
    }

    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfigurationSection jwtSection)
    {
        var jwtSettings = jwtSection.Get<JwtSettings>();
        
        services
            .Configure<JwtSettings>(jwtSection)
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidAudience = jwtSettings?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                    
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        services.AddAuthorization();
        
        return services;
    }
}
