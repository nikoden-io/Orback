using Application.Abstractions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(o =>
            o.UseNpgsql(config.GetConnectionString("db")));

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddSerilog((sp, lc) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            lc.ReadFrom.Configuration(configuration)
              .Enrich.FromLogContext();
        });

        return services;
    }
}