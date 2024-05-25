namespace Infrastructure.Postgres.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _context;

    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Commit() => await _context.SaveChangesAsync();

    public async Task Rollback() => await _context.DisposeAsync();
}