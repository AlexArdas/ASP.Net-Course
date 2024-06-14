using System.Linq.Expressions;
using CST.Common.Models.Enums;

namespace CST.Common.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string? sortByField, SortOrder? sortOrder = SortOrder.Asc)
        {
            if (string.IsNullOrEmpty(sortByField) || sortOrder is null)
            {
                return source;
            }

            var parameter = Expression.Parameter(source.ElementType, string.Empty);

            var property = Expression.Property(parameter, sortByField);

            var lambda = Expression.Lambda(property, parameter);

            var methodName = sortOrder switch
            {
                SortOrder.Asc => "OrderBy",
                SortOrder.Desc => "OrderByDescending",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(methodName))
            {
                return source;
            }

            var methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                new[] { source.ElementType, property.Type },
                source.Expression, Expression.Quote(lambda));

            source = source.Provider.CreateQuery<T>(methodCallExpression);

            return source;
        }
    }
}
