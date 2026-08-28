using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Common;

namespace UverTeaServerApp.Shared.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Projects an IQueryable to a PagedResult asynchronously.
    /// </summary>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var effectivePageNumber = pageNumber < 1 ? 1 : pageNumber;
        var effectivePageSize = pageSize < 1 ? 10 : pageSize;

        var items = await query
            .Skip((effectivePageNumber - 1) * effectivePageSize)
            .Take(effectivePageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, effectivePageNumber, effectivePageSize, totalCount);
    }

    /// <summary>
    /// Overload that takes PaginationParams directly.
    /// </summary>
    public static Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationParams? paginationParams,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = paginationParams?.PageNumber ?? 1;
        var pageSize = paginationParams?.PageSize ?? 10;

        return query.ToPagedResultAsync(pageNumber, pageSize, cancellationToken);
    }

    /// <summary>
    /// Applies dynamic sorting to an IQueryable by property name.
    /// </summary>
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> query,
        string? sortColumn,
        string? sortDirection = "asc")
    {
        if (string.IsNullOrWhiteSpace(sortColumn))
        {
            return query;
        }

        var entityType = typeof(T);
        var property = entityType.GetProperty(
            sortColumn,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property == null)
        {
            return query;
        }

        var parameter = Expression.Parameter(entityType, "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);

        var isDescending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
        var methodName = isDescending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);

        var resultExp = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { entityType, property.PropertyType },
            query.Expression,
            Expression.Quote(orderByExp));

        return query.Provider.CreateQuery<T>(resultExp);
    }
}
