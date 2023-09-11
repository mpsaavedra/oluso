#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Oluso.Extensions;

/// <summary>
/// 
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="where"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> ToWhere<T>(this IQueryable<T> queryable, Expression<Func<T, bool>>? where)
        where T : class
    {
        if (where != default)
        {
            queryable = queryable.Where(where);
        }

        return queryable;
    }
    
    private static IQueryable<T> ToOrder<T>(
            this IQueryable<T> queryable,
            string property,
            bool ascending)
        {
            Is.NullOrAnyNull(queryable, nameof(queryable));
            Is.NullOrEmpty(property, nameof(property));

            var properties = property.Split('.');

            Type? propertyType = typeof(T);

            propertyType = properties.Aggregate(propertyType, (Type? propertyTypeCurrent, string propertyName) =>
                propertyTypeCurrent?.GetProperty(propertyName)?.PropertyType);

            if (propertyType == typeof(sbyte))
            {
                return queryable.ToOrder<T, sbyte>(properties, ascending);
            }

            if (propertyType == typeof(short))
            {
                return queryable.ToOrder<T, short>(properties, ascending);
            }

            if (propertyType == typeof(int))
            {
                return queryable.ToOrder<T, int>(properties, ascending);
            }

            if (propertyType == typeof(long))
            {
                return queryable.ToOrder<T, long>(properties, ascending);
            }

            if (propertyType == typeof(byte))
            {
                return queryable.ToOrder<T, byte>(properties, ascending);
            }

            if (propertyType == typeof(ushort))
            {
                return queryable.ToOrder<T, ushort>(properties, ascending);
            }

            if (propertyType == typeof(uint))
            {
                return queryable.ToOrder<T, uint>(properties, ascending);
            }

            if (propertyType == typeof(ulong))
            {
                return queryable.ToOrder<T, ulong>(properties, ascending);
            }

            if (propertyType == typeof(char))
            {
                return queryable.ToOrder<T, char>(properties, ascending);
            }

            if (propertyType == typeof(float))
            {
                return queryable.ToOrder<T, float>(properties, ascending);
            }

            if (propertyType == typeof(double))
            {
                return queryable.ToOrder<T, double>(properties, ascending);
            }

            if (propertyType == typeof(decimal))
            {
                return queryable.ToOrder<T, decimal>(properties, ascending);
            }

            if (propertyType == typeof(bool))
            {
                return queryable.ToOrder<T, bool>(properties, ascending);
            }

            if (propertyType == typeof(string))
            {
                return queryable.ToOrder<T, string>(properties, ascending);
            }

            return queryable.ToOrder<T, object>(properties, ascending);
        }

        private static IQueryable<T> ToOrder<T, TKey>(
            this IQueryable<T> queryable,
            IEnumerable<string> properties,
            bool ascending)
        {
            Is.NullOrAnyNull(queryable, nameof(queryable));
            Is.NullOrAnyNull(properties, nameof(properties));

            var parameters = Expression.Parameter(typeof(T));

            var body = properties.Aggregate<string, Expression>(parameters, Expression.Property);

            var expression = Expression.Lambda<Func<T, TKey>>(body, parameters).Compile();

            return ascending ? queryable.AsEnumerable().OrderBy(expression).AsQueryable() : queryable.AsEnumerable().OrderByDescending(expression).AsQueryable();
        }
}