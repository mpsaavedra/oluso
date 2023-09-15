using System.Linq.Expressions;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Oluso.Data.Operations;
using Oluso.Data.QueryResponses;
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
    IQueryRepository<TKey, TUserKey, TEntity, TContext>,
    ICommandRepository<TKey, TUserKey, TEntity, TContext>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    /// <summary>
    /// get the query repository <see cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}"/>
    /// </summary>
    IQueryRepository<TKey, TUserKey, TEntity, TContext> QueryRepository { get; }
    
    /// <summary>
    /// get the command repository <see cref="ICommandRepository{TKey,TUserKey,TEntity,TContext}"/>
    /// </summary>
    ICommandRepository<TKey, TUserKey, TEntity, TContext> CommandRepository { get; }
}

/// <inheritdoc cref="IRepository{TKey,TUserKey,TEntity,TContext}"/>
public class Repository<TKey, TUserKey, TEntity, TContext> :
    IRepository<TKey, TUserKey, TEntity, TContext>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    private IRepository<TKey, TUserKey, TEntity, TContext> _repositoryImplementation;

    /// <summary>
    /// returns a new <see cref="Repository{TKey,TUserKey,TEntity,TContext}"/> instance
    /// </summary>
    /// <param name="unitOfWork"></param>
    public Repository(IUnitOfWork<TContext> unitOfWork)
    {
        Insist.MustBe.NotNull(unitOfWork);
        UnitOfWork = unitOfWork;
        CommandRepository = new CommandRepository<TKey, TUserKey, TEntity, TContext>(unitOfWork);
        QueryRepository = new QueryRepository<TKey, TUserKey, TEntity, TContext>(unitOfWork.Context);
        Query = QueryRepository.Query;
        Context = unitOfWork.Context;
    }

    /// <inheritdoc cref="IRepository{TKey,TUserKey,TEntity,TContext}.QueryRepository"/>
    public IQueryRepository<TKey, TUserKey, TEntity, TContext> QueryRepository { get; }

    /// <inheritdoc cref="CommandRepository"/>
    public ICommandRepository<TKey, TUserKey, TEntity, TContext> CommandRepository { get; }

    /// <inheritdoc cref="ICreateOperation{TKey,TUserKey,TEntity}.CreateAsync"/>
    public async Task<TKey>? CreateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await CommandRepository.CreateAsync(entity, cancellationToken)!;

    /// <inheritdoc cref="ICreateOperation{TKey,TUserKey,TEntity}.CreateRangeAsync"/>
    public async Task<List<TKey>>? CreateRangeAsync(List<TEntity> entities,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.CreateRangeAsync(entities, cancellationToken)!;

    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindAsync(System.Linq.Expressions.Expression{System.Func{TEntity,bool}},System.Threading.CancellationToken)"/>
    public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await QueryRepository.FindAsync(predicate, cancellationToken);

    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindAsync(Oluso.Data.Specifications.Specification{TEntity},System.Threading.CancellationToken)"/>
    public async Task<IQueryable<TEntity>> FindAsync(Specification<TEntity> specification,
        CancellationToken cancellationToken = default) =>
        await QueryRepository.FindAsync(specification, cancellationToken);

    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindPaginatedAsync(System.Linq.Expressions.Expression{System.Func{TEntity,bool}},int,int,System.Threading.CancellationToken)"/>
    public async Task<QueryResult<TEntity>> FindPaginatedAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex = 0,
        int pageSize = 50,
        CancellationToken cancellationToken = default) =>
        await QueryRepository.FindPaginatedAsync(predicate, pageIndex, pageSize, cancellationToken);

    /// <inheritdoc cref="IReadOperation{TKey,TUserKey,TEntity}.FindPaginatedAsync(Oluso.Data.Specifications.Specification{TEntity},int,int,System.Threading.CancellationToken)"/>
    public async Task<QueryResult<TEntity>> FindPaginatedAsync(Specification<TEntity> specification, int pageIndex = 0,
        int pageSize = 50,
        CancellationToken cancellationToken = default) =>
        await QueryRepository.FindPaginatedAsync(specification, pageIndex, pageSize, cancellationToken);

    /// <inheritdoc cref="IUpdateOperation{TKey,TUserKey,TEntity}.UpdateAsync"/>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await CommandRepository.UpdateAsync(entity, cancellationToken);

    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteAsync(System.Linq.Expressions.Expression{System.Func{TEntity,bool}},bool,System.Threading.CancellationToken)"/>
    public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool softDelete = true,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.DeleteAsync(predicate, softDelete, cancellationToken);

    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteAsync(TKey,bool,System.Threading.CancellationToken)"/>
    public async Task<bool> DeleteAsync(TKey id, bool softDelete = true,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.DeleteAsync(id, softDelete, cancellationToken);

    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteAsync(TEntity,bool,System.Threading.CancellationToken)"/>
    public async Task<bool> DeleteAsync(TEntity entity, bool softDelete = true,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.DeleteAsync(entity, softDelete, cancellationToken);

    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteRangeAsync(System.Collections.Generic.List{TEntity},bool,System.Threading.CancellationToken)"/>
    public async Task<bool> DeleteRangeAsync(List<TEntity> entities, bool softDelete = true,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.DeleteRangeAsync(entities, softDelete, cancellationToken);

    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteRangeAsync(System.Collections.Generic.List{TKey},bool,System.Threading.CancellationToken)"/>
    public async Task<bool> DeleteRangeAsync(List<TKey> entitiesIds, bool softDelete = true,
        CancellationToken cancellationToken = default) =>
        await CommandRepository.DeleteRangeAsync(entitiesIds, softDelete, cancellationToken);

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Query"/>
    public IQueryable<TEntity> Query { get; }
    
    /// <inheritdoc cref="ICommandRepository{TKey,TUserKey,TEntity,TContext}.UnitOfWork"/>
    public IUnitOfWork<TContext>? UnitOfWork { get; }
    
    /// <inheritdoc cref="ICommandRepository{TKey,TUserKey,TEntity,TContext}.Context"/>
    public TContext Context { get; }

    /// <inheritdoc cref="ICommandRepository{TKey,TUserKey,TEntity,TContext}.ChangeStatusAsync"/>
    public async Task<bool>? ChangeStatusAsync(TKey id, bool state, CancellationToken cancellationToken = default) =>
        await CommandRepository.ChangeStatusAsync(id, state, cancellationToken)!;

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.GetEntity{T}"/>
    public DbSet<T> GetEntity<T>() where T : class =>
        QueryRepository.GetEntity<T>();

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Count"/>
    public int Count(bool softDeleted = false) =>
        QueryRepository.Count(softDeleted);

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.CountAsync"/>
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default) =>
        await QueryRepository.CountAsync(predicate, cancellationToken);

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.Any"/>
    public bool Any(bool softDeleted = false) =>
        QueryRepository.Any(softDeleted);

    /// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}.AnyAsync"/>
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default) =>
        await QueryRepository.AnyAsync(predicate, cancellationToken);
}