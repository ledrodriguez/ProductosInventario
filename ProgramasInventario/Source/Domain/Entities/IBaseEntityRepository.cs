using System.Linq.Expressions;

namespace Domain.Entities;

public interface IBaseEntityRepository<T> where T : BaseEntity
{
    Task Commit();
    void DeleteMany(IEnumerable<T> entities);
    void DeleteOne(T entity);
    Task<bool> ExistsBy(Expression<Func<T, bool>> filter = null);
    Task InsertMany(IEnumerable<T> entities);
    Task InsertOne(T entity);

    Task<(List<TP> data, int total)> ListManyBy<TP>(Expression<Func<T, TP>> project, BaseListingFiltersDto<T> filter)
        where TP : BaseListedProjectionDto;

    Task<IList<TP>> ProjectManyBy<TP>(Expression<Func<T, TP>> project, Expression<Func<T, bool>> filter = null);
    Task<TP> ProjectOneBy<TP>(Expression<Func<T, TP>> project, Expression<Func<T, bool>> filter = null);
    Task<IList<T>> SelectManyBy(Expression<Func<T, bool>> filter = null, bool track = false);
    Task<T> SelectOneBy(Expression<Func<T, bool>> filter = null, bool track = false);
    void UpdateMany(IEnumerable<T> entities);
    void UpdateOne(T entity);
}