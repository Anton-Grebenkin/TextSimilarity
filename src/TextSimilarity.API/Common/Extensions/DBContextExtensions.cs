using System.Linq.Expressions;
using TextSimilarity.API.Common.DataAccess;

namespace TextSimilarity.API.Common.Extensions
{
    public static class DBContextExtensions
    {
        private const string ORDER_BY_DESCENDING = nameof(Queryable.OrderByDescending);
        private const string ORDER_BY = nameof(Queryable.OrderBy);
        private const string THEN_BY = nameof(Queryable.ThenBy);
        private const string THEN_BY_DESCENDING = nameof(Queryable.ThenByDescending);
        public static IQueryable<T> OrderBySorts<T>(this IQueryable<T> query, ColumnSort[] sorts)  where T : class
        {
            if (!sorts.Any())
                return query;

            var queryExpr = query.Expression;

            for (var i = 0; i < sorts.Length; i++)
            {
                var sort = sorts[i];

                var command = "";
                if (sort.Desc)
                {
                    if (i == 0)
                        command = ORDER_BY_DESCENDING;
                    else
                        command = THEN_BY_DESCENDING;
                }
                else
                {
                    if (i == 0)
                        command = ORDER_BY;
                    else
                        command = THEN_BY;
                }

                var propertyName = sort.Id;

                var type = typeof(T);
                var property = type.GetProperties()
                    .Where(item => item.Name.ToLower() == propertyName.ToLower())
                    .FirstOrDefault();

                if (property == null)
                    continue;

                var parameter = Expression.Parameter(type, "p");

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);

                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                queryExpr = Expression.Call(
                    type: typeof(Queryable),
                    methodName: command,
                    typeArguments: new Type[] { type, property.PropertyType },
                    queryExpr,
                    Expression.Quote(orderByExpression));

                query = query.Provider.CreateQuery<T>(queryExpr);
            }

            return query;
        }
    }
}
