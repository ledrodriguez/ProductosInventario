using System.Linq.Expressions;
using Domain.Common.Pages;

namespace Domain.Entities;

public abstract class BaseListingFiltersDto<T> : PageConfiguration where T : BaseEntity
{
    public Guid? Id { get; init; }
    public DateTime? CreatedAt { get; init; }
    public string CreatedBy { get; init; }
    public DateTime? LastUpdatedAt { get; init; }
    public string LastUpdatedBy { get; init; }
    public bool? Active { get; init; }

    public IDictionary<Expression<Func<T, bool>>, bool> PropertyFilters { get; } =
        new Dictionary<Expression<Func<T, bool>>, bool>();
}