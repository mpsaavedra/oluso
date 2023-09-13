using System.Linq.Expressions;
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
    public Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// search for entities that match the provided <see cref="Specification{TEntity}"/>
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<IQueryable<TEntity>> FindAsync(Specification<TEntity> specification,
        CancellationToken cancellationToken = default);
}