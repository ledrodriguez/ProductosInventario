using Domain;
using Domain.Entities;
using Infrastructure.Postgres;
using Infrastructure.Postgres.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddPostgres();
    }

    internal static void AddPostgres(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionStringBuilder, ConnectionStringBuilder>();

        var connectionString = services
            .BuildServiceProvider()
            .GetRequiredService<IConnectionStringBuilder>()
            .Build();

        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(connectionString, action =>
                action.MigrationsAssembly("Infrastructure")));

        services.AddHealthChecks()
            .AddDbContextCheck<DatabaseContext>()
            .AddNpgSql(connectionString);

        services.AddScoped(typeof(IBaseEntityRepository<>), typeof(BaseEntityRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        if (!GlobalConfigurations.IsDevelopment)
            services.BuildServiceProvider().GetRequiredService<DatabaseContext>().Database.Migrate();
    }
}