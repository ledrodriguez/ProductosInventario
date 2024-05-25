using Application;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public static class DependencyInjection
{
    public static void AddIntegration(this IServiceCollection services)
    {
        Environment.SetEnvironmentVariable("POSTGRESQL_DATABASE", "integration-tests-template");

        services.AddServices();
        services.AddPostgres();
    }
}