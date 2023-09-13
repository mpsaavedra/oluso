using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Oluso.Data.Operations;
using Oluso.Data.Specifications;

namespace Oluso.Data.Repositories;

/// <summary>
/// Command and Query repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TUserKey"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TContext"></typeparam>
public interface IRepository<TKey, TUserKey, TEntity, TContext> :
    ICreateOperation<TKey, TUserKey, TEntity>,
    IReadOperation<TKey, TUserKey, TEntity>,
    IUpdateOperation<TKey, TUserKey, TEntity>,
    IDeleteOperation<TKey, TUserKey, TEntity>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    /// <summary>
    /// get the query repository <see cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}"/>
    /// </summary>
    IQueryRepository<TKey, TUserKey, TEntity, TContext> QueryRepositsory { get; }
    
    /// <summary>
    /// get the command repository <see cref="ICommandRepository{TKey,TUserKey,TEntity,TContext}"/>
    /// </summary>
    ICommandRepository<TKey, TUserKey, TEntity, TContext> CommandRepository { get; }
}

/// <summary>
/// <inheritdoc cref="IRepository{TKey,TUserKey,TEntity,TContext}"/>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TUserKey"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TContext"></typeparam>
public class Repository<TKey, TUserKey, TEntity, TContext> :
    IRepository<TKey, TUserKey, TEntity, TContext>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    /// <summary>
    /// returns a new <see cref="Repository{TKey,TUserKey,TEntity,TContext}"/> instance
    /// </summary>
    /// <param name="unitOfWork"></param>
    public Repository(IUnitOfWork<TContext> unitOfWork)
    {
        Insist.MustBe.NotNull(unitOfWork);
        CommandRepository = new CommandRepository<TKey, TUserKey, TEntity, TContext>(unitOfWork);
        QueryRepositsory = new QueryRepository<TKey, TUserKey, TEntity, TContext>(unitOfWork.Context);
    }

    /// <summary>
    /// <inheritdoc cref="IRepository{TKey,TUserKey,TEntity,TContext}.QueryRepositsory"/>
    /// </summary>
    public IQueryRepository<TKey, TUserKey, TEntity, TContext> QueryRepositsory { get; }

    /// <summary>
    /// <inheritdoc cref="CommandRepository"/>
    /// </summary>
    public ICommandRepository<TKey, TUserKey, TEntity, TContext> CommandRepository { get; }

    /// <summary>
    /// <inheritdoc cref="ICreateOperation{TKey,TUserKey,TEntity}.CreateAsync"/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TKey>? CreateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await CommandRepository.CreateAsync(entity, cancellationToken)!;

    /// <summary>
    /// <inheritdoc cref="ICreateOperation{TKey,TUserKey,TEntity}.CreateRangeAsync"/>
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<TKey>>? CreateRangeAsync(List<TEntity> entities,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.CreateRangeAsync(entities, cancellationToken)!;

    /// <summary>
    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindAsync(System.Linq.Expressions.Expression{System.Func{TEntity,bool}},System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await QueryRepositsory.FindAsync(predicate, cancellationToken);

    public async Task<IQueryable<TEntity>> FindAsync(Specification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteAsync(System.Linq.Expressions.Expression{System.Func{TEntity,bool}},bool,System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool softDelete = true,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.DeleteAsync(predicate, softDelete, cancellationToken);

    /// <summary>
    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteAsync(TKey,bool,System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(TKey id, bool softDelete = true,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.DeleteAsync(id, softDelete, cancellationToken);

    /// <summary>
    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteAsync(TEntity,bool,System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(TEntity entity, bool softDelete = true,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.DeleteAsync(entity, softDelete, cancellationToken);

    /// <summary>
    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteRangeAsync(System.Collections.Generic.List{TEntity},bool,System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteRangeAsync(List<TEntity> entities, bool softDelete = true,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.DeleteRangeAsync(entities, softDelete, cancellationToken);

    /// <summary>
    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteRangeAsync(System.Collections.Generic.List{TKey},bool,System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="entitiesIds"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteRangeAsync(List<TKey> entitiesIds, bool softDelete = true,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.DeleteRangeAsync(entitiesIds, softDelete, cancellationToken);
}