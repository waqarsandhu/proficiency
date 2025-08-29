using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PM.Common.Common
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (page <= 0) page = Constants.DefaultPage;
            if (pageSize <= 0) pageSize = Constants.DefaultPageSize;

            long skipCount = ((long)(page - 1)) * pageSize;

            if (skipCount > int.MaxValue)
                skipCount = int.MaxValue;

            return query.Skip((int)skipCount).Take(pageSize);
        }


        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPagedRequest request)
        {
            return query.PageBy(request.Page, request.PageSize);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, ISortedResultRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SortBy))
            {
                return source;
            }

            bool descending = request.SortOrder?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;
            string methodName = descending ? "OrderByDescending" : "OrderBy";

            var parameter = Expression.Parameter(typeof(T), "x");
            var body = BuildPropertyPath(parameter, request.SortBy);

            var lambda = Expression.Lambda(body, parameter);
            var methodInfo = typeof(Queryable).GetMethods()
                                              .First(m => m.Name == methodName &&
                                                          m.GetParameters().Length == 2)
                                              .MakeGenericMethod(typeof(T), body.Type);

            return (IQueryable<T>)methodInfo.Invoke(null, new object[] { source, lambda });
        }

        private static Expression BuildPropertyPath(Expression root, string propertyPath)
        {
            foreach (var member in propertyPath.Split('.'))
            {
                root = Expression.PropertyOrField(root, member);
            }
            return root;
        }


        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }
    }
}
