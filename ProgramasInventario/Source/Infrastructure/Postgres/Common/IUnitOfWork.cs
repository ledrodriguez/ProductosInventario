namespace Infrastructure.Postgres.Common;

public interface IUnitOfWork
{
    Task<int> Commit();
    Task Rollback();
}