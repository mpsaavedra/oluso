using System.Linq.Expressions;

namespace Oluso.Data.Operations;

/// <summary>
/// Delete operations
/// </summary>
public interface IDeleteOperation<TKey, TUserKey, TEntity>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
{
    /// <summary>
    /// delete the first matching entity. This operation will save changes into the database.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool softDelete = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// delete the entity with provided Id, This operation will save changes into the database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(TKey id, bool softDelete = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// deletes the entity, this operation will save changes into the database
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(TEntity entity, bool softDelete = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// deletes a range of entities from database.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> DeleteRangeAsync(List<TEntity> entities, bool softDelete = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// delete a range of entities by their ids. This operation save changes to the database.
    /// </summary>
    /// <param name="entitiesIds"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> DeleteRangeAsync(List<TKey> entitiesIds, bool softDelete = true,
        CancellationToken cancellationToken = default);
}