#nullable enable
using System;
using System.Linq;
using System.Linq.Expressions;
using AgileObjects.AgileMapper;
using AgileObjects.AgileMapper.Api.Configuration;
using AgileObjects.AgileMapper.Api.Configuration.Projection;
using AgileObjects.AgileMapper.Extensions;

namespace Oluso.Extensions;

/// <summary>
/// Agile Mapper related extensions
/// </summary>
public static class AgileMapperExtensions
{
    /// <summary>
    /// creates a deep clone of the object using the <see cref="IFullMappingInlineConfigurator{TSource,TTarget}"/>
    /// provided configurations
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurations"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource ToMapDeepClone<TSource>(this TSource source,
        params Expression<Action<IFullMappingInlineConfigurator<TSource, TSource>>>[] configurations)
        where TSource : class =>
        source.DeepClone(configurations);

    /// <summary>
    /// creates a completely new object from source using <see cref="IFullMappingInlineConfigurator{TSource,TTarget}"/>
    /// options
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="TDestination"></typeparam>
    /// <returns></returns>
    public static TDestination ToMapCreateNew<TDestination>(this object source,
        Expression<Action<IFullMappingInlineConfigurator<object, TDestination>>> configuration)
        where TDestination : class =>
        source.Map().ToANew(configuration);

    /// <summary>
    /// updates the destination object by mapping data from source into it
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="configurations"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <returns></returns>
    public static TDestination ToMapUpdate<TSource, TDestination>(this TSource source, TDestination destination,
        params Expression<Action<IFullMappingInlineConfigurator<TSource, TDestination>>>[] configurations)
        where TSource : class
        where TDestination : class =>
        source.Map().Over(destination, configurations);

    /// <summary>
    /// merge source object onto the destination object
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="configurations"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <returns></returns>
    public static TDestination ToMapMerge<TSource, TDestination>(this TSource source, TDestination destination,
        params Expression<Action<IFullMappingInlineConfigurator<TSource, TDestination>>>[] configurations)
        where TSource : class
        where TDestination : class =>
        source.Map().OnTo(destination, configurations);

    /// <summary>
    /// Map a list into another list
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <returns></returns>
    public static IQueryable<TDestination> ToMapList<TSource, TDestination>(this IQueryable<TSource> queryable,
        Expression<Action<IFullProjectionInlineConfigurator<TSource, TDestination>>>? configuration = null)
        where TSource : class
        where TDestination : class =>
        configuration is null ? queryable.Project().To<TDestination>() : queryable.Project().To(configuration);
}