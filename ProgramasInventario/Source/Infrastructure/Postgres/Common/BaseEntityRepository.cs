using System.Linq.Expressions;
using Domain.Common.Extensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Postgres.Common;

public class BaseEntityRepository<T> : IBaseEntityRepository<T> where T : BaseEntity
{
    private readonly DatabaseContext _context;
    private readonly DbSet<T> _entity;

    public BaseEntityRepository(DatabaseContext context)
    {
        _context = context;
        _entity = _context.Set<T>();
    }

    public async Task Commit() => await _context.SaveChangesAsync();

    public void DeleteMany(IEnumerable<T> entities) => UpdateMany(entities);

    public void DeleteOne(T entity) => UpdateOne(entity);

    public async Task<bool> ExistsBy(Expression<Func<T, bool>> filter = null) =>
        await _entity.AsNoTracking().AnyAsync(filter ?? (x => true));

    public async Task InsertMany(IEnumerable<T> entities) => await _entity.AddRangeAsync(entities);

    public async Task InsertOne(T entity) => await _entity.AddAsync(entity);

    public async Task<(List<TP> data, int total)> ListManyBy<TP>(
        Expression<Func<T, TP>> project, BaseListingFiltersDto<T> filter) where TP : BaseListedProjectionDto
    {
        var query = _entity.AsQueryable();

        if (filter.PropertyFilters.Any())
            foreach (var (propertyFilter, condition) in filter.PropertyFilters)
                if (condition)
                    query = query.Where(propertyFilter);

        if (filter.Order.Any())
            foreach (var (property, ascendant) in filter.Order)
                query = query.OrderBy(property, ascendant);

        var data = await query.AsNoTracking().PaginateBy(filter.Page, filter.Size).Select(project).ToListAsync();
        var total = await query.CountAsync();

        return (data, total);
    }

    public async Task<IList<TP>> ProjectManyBy<TP>(
        Expression<Func<T, TP>> project, Expression<Func<T, bool>> filter = null) =>
        await _entity.AsNoTracking().Where(filter ?? (x => true)).Select(project).ToListAsync();

    public async Task<TP> ProjectOneBy<TP>(Expression<Func<T, TP>> project, Expression<Func<T, bool>> filter = null) =>
        await _entity.AsNoTracking().Where(filter ?? (x => true)).Select(project).SingleOrDefaultAsync();

    public async Task<IList<T>> SelectManyBy(Expression<Func<T, bool>> filter = null, bool track = false) =>
        track
            ? await _entity.Where(filter ?? (x => true)).ToListAsync()
            : await _entity.AsNoTracking().Where(filter ?? (x => true)).ToListAsync();

    public async Task<T> SelectOneBy(Expression<Func<T, bool>> filter = null, bool track = false) =>
        track
            ? await _entity.SingleOrDefaultAsync(filter ?? (x => true))
            : await _entity.AsNoTracking().SingleOrDefaultAsync(filter ?? (x => true));

    public void UpdateMany(IEnumerable<T> entities) => _entity.UpdateRange(entities);

    public void UpdateOne(T entity) => _entity.Update(entity);
}