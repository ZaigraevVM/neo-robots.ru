using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System;

namespace SMI.Code.Extensions
{
    public static class LinqExtension
    {
        public static IOrderedQueryable<TSource> OrderByFilter<TSource>(this IQueryable<TSource> query, string sortField)
        {
            if (!string.IsNullOrEmpty(sortField))
            {
                return query.OrderBy(sortField);
            }

            return (IOrderedQueryable<TSource>)query;
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName, string sortOrder = "asc", string defaultPropertyName = "id")
        {
            var propertyInfo = GetPropertyInfoByNameOrDefaultName<TSource>(propertyName, defaultPropertyName);

            if (propertyInfo == null)
            {
                return (IOrderedQueryable<TSource>)query;
            }

            var order = sortOrder == "desc" ? "OrderByDescending" : "OrderBy";

            var entityType = typeof(TSource);
            var arg = Expression.Parameter(entityType, "x");
            var property = Expression.Property(arg, propertyInfo.Name);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            var method = typeof(Queryable).GetMethods()
                 .Where(m => m.Name == order && m.IsGenericMethodDefinition)
                 .Where(m => m.GetParameters().ToList().Count == 2)
                 .Single();

            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            return (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
        }

        public static PropertyInfo GetPropertyInfoByNameOrDefaultName<TSource>(string propertyName, string defaultPropertyName)
        {
            return GetPropertyInfoByName<TSource>(propertyName) ?? GetPropertyInfoByName<TSource>(defaultPropertyName);
        }

        public static PropertyInfo GetPropertyInfoByName<TSource>(string propertyName)
        {
            var name = propertyName?.Trim();

            if (string.IsNullOrEmpty(name)) 
                return null;

            return typeof(TSource).GetProperties().ToList().Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
