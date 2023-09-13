namespace Oluso.Data.Operations;

/// <summary>
/// Create operations (Command)
/// </summary>
public interface ICreateOperation<TKey, TUserKey, TEntity>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
{
    /// <summary>
    /// Creates a new entity and the returns the Id. This this operation automatically save changes into the
    /// database after adding the entity
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TKey>? CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a range of new entities and returns a list with the Ids, This operation will automatically save changes
    /// into the database after adding the entity
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TKey>>? CreateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
}