using System.Linq.Expressions;

namespace Domain.Common.Extensions;

public static class QueryableExtension
{
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, bool ascending)
        where T : class
    {
        var entityType = typeof(T);

        var propertyInfo = entityType.GetProperty(GlobalConfigurations.CultureInfo.TextInfo.ToTitleCase(propertyName));
        if (propertyInfo == null)
            return query;

        var parameterExpression = Expression.Parameter(entityType, "x");
        var memberExpression = Expression.Property(parameterExpression, propertyName);
        var lambdaExpression = Expression.Lambda(memberExpression, parameterExpression);

        var genericMethodInfo = typeof(Queryable).GetMethods()
            .Where(x => x.Name == (ascending ? "OrderBy" : "OrderByDescending") && x.IsGenericMethodDefinition)
            .Single(x => x.GetParameters().ToList().Count == 2)
            .MakeGenericMethod(entityType, propertyInfo.PropertyType);

        return (IQueryable<T>)genericMethodInfo.Invoke(genericMethodInfo, new object[] { query, lambdaExpression });
    }

    public static IQueryable<T> PaginateBy<T>(this IQueryable<T> query, int page, int size) =>
        query.Skip((page - 1) * size).Take(size);
}