using System.Linq.Expressions;
using Oluso.Data.QueryResponses;
using Oluso.Data.Specifications;

namespace Oluso.Data.Operations;

/// <summary>
/// Simple query operations
/// </summary>
public interface IReadOperation<TKey, TUserKey, TEntity>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
{
    /// <summary>
    /// search for entities that match the provided predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// search for entities that match the provided <see cref="Specification{TEntity}"/>
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IQueryable<TEntity>> FindAsync(Specification<TEntity>? specification = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// returns a paginated <see cref="QueryResult{TEntity}"/> object with the
    /// result of que Query
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="pageIndex">default 0 or the page index to display, greater or equal to 0</param>
    /// <param name="pageSize">default 50 or any other custom value greater or equal to 1</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<QueryResult<TEntity>> FindPaginatedAsync(Expression<Func<TEntity, bool>>? predicate = null,
        int pageIndex = 0, int pageSize = 50, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// returns a paginated <see cref="QueryResult{TEntity}"/> object with the result of the Query
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="pageIndex">default 0 or the page index to display, greater or equal to 0</param>
    /// <param name="pageSize">default 50 or any other custom value greater or equal to 1</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<QueryResult<TEntity>> FindPaginatedAsync(Specification<TEntity>? specification = null,
        int pageIndex = 0, int pageSize = 50,
        CancellationToken cancellationToken = default);
}