using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Oluso.Data.Operations;

namespace Oluso.Data.Repositories;

/// <summary>
/// A query only repository, this repository is meant to be used for read only operations. <b>Notice
/// that all query operations return by default those entities marked with status true, so they
/// are active to the system</b>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TUserKey"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TContext"></typeparam>
public interface IQueryRepository<TKey, TUserKey, TEntity, TContext> : IReadOperation<TKey, TUserKey, TEntity>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    /// <summary>
    /// gets a Queryable result of the TEntity DbSet to make read operations
    /// </summary>
    IQueryable<TEntity> Query { get; }

    /// <summary>
    /// returns the DbContext
    /// </summary>
    TContext Context { get; }
    
    /// <summary>
    /// returns an entity's DbSet of type TEntity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    DbSet<T> GetEntity<T>() where T : class;

    /// <summary>
    /// returns the how many elements are of TEntity
    /// </summary>
    /// <returns></returns>
    int Count(bool softDeleted = false);

    /// <summary>
    /// returns the how many elements are of TEntity
    /// </summary>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// returns true if theres any element of TEntity
    /// </summary>
    /// <returns></returns>
    bool Any(bool softDeleted = false);

    /// <summary>
    /// returns true if theres any element of TEntity
    /// </summary>
    /// <returns></returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
}