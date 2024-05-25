using Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration;

public abstract class IntegrationBase : IDisposable
{
    private DatabaseContext Context => _context.Value;
    private readonly Lazy<DatabaseContext> _context;
    private readonly Lazy<IServiceScope> _scope;

    protected IntegrationBase()
    {
        var services = BuildServiceCollection();
        var provider = new Lazy<IServiceProvider>(() =>
            new DefaultServiceProviderFactory(new ServiceProviderOptions()).CreateServiceProvider(services));
        _scope = new Lazy<IServiceScope>(() => provider.Value.CreateScope());
        _context = new Lazy<DatabaseContext>(GetRequiredService<DatabaseContext>);

        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    protected async Task Commit() => await Context.SaveChangesAsync();

    protected T GetRequiredService<T>() => _scope.Value.ServiceProvider.GetRequiredService<T>();

    private static IServiceCollection BuildServiceCollection()
    {
        var services = new ServiceCollection();
        services.AddIntegration();
        services.AddLogging(x => x.AddConsole());
        return services;
    }

    public void Dispose()
    {
        if (_scope.IsValueCreated)
            _scope.Value.Dispose();

        GC.SuppressFinalize(this);
    }
}