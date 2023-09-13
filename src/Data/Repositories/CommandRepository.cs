using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Oluso.Data.Operations;

namespace Oluso.Data.Repositories;

/// <summary>
/// <inheritdoc cref="ICommandRepository{TKey,TUserKey,TEntity,TContext}"/>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TUserKey"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TContext"></typeparam>
public class CommandRepository<TKey, TUserKey, TEntity, TContext> :
    ICommandRepository<TKey, TUserKey, TEntity, TContext>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    /// <summary>
    /// returns a new <see cref="CommandRepository{TKey,TUserKey,TEntity,TContext}"/> instance
    /// </summary>
    /// <param name="unitOfWork"></param>
    public CommandRepository(IUnitOfWork<TContext> unitOfWork)
    {
        Insist.MustBe.NotNull(unitOfWork);
        UnitOfWork = unitOfWork;
        Context = unitOfWork.Context;
    }
    
    /// <summary>
    /// <inheritdoc cref="ICommandRepository{TKey,TUserKey,TEntity,TContext}.UnitOfWork"/>
    /// </summary>
    public IUnitOfWork<TContext>? UnitOfWork { get; }

    /// <summary>
    /// <inheritdoc cref="ICommandRepository{TKey,TUserKey,TEntity,TContext}.Context"/>
    /// </summary>
    public TContext Context { get; }

    /// <summary>
    /// <inheritdoc cref="ICommandRepository{TKey,TUserKey,TEntity,TContext}.ChangeStatusAsync"/>.
    /// This function always return a valid value, if result is false you must probably check the Insist
    /// registry for a more detailed information about why the value is false. 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool>? ChangeStatusAsync(TKey id, bool state, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
            if (entity == null)
            {
                throw new Exception(Messages.EntityWithIdNotFound(id));
            }

            entity!.Status = state;
            await UpdateAsync(entity, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            Insist.RegisterException(e, e.Message);
            return false;
        }
    }

    /// <summary>
    /// <inheritdoc cref="ICreateOperation{TKey,TUserKey,TEntity}.CreateAsync"/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TKey>? CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            if (UnitOfWork == null)
            {
                var ent = await Context.AddAsync(entity, cancellationToken);
                await Context.SaveChangesAsync(cancellationToken);
                return await Task.FromResult(ent.Entity.Id);
            }
            
            return (await UnitOfWork!.ExecuteAsync(async () => 
                        await Context.AddAsync(entity, cancellationToken), 
                        cancellationToken: cancellationToken)).Entity.Id;
        }
        catch (Exception e)
        {
            Insist.RegisterException(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// <inheritdoc cref="ICreateOperation{TKey,TUserKey,TEntity}.CreateRangeAsync"/>
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<TKey>>? CreateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = new List<TKey>();
            if (UnitOfWork == null)
            {
                foreach (var entity in entities)
                {
                    var id = await Context.AddAsync(entity, cancellationToken);
                    result.Add(id.Entity.Id);
                }
                await Context.SaveChangesAsync(cancellationToken);
                return result;
            }

            foreach (var entity in entities)
            {
                var id = await CreateAsync(entity, cancellationToken)!;
                result.Add(id);
            }
            
            return result;
        }
        catch (Exception e)
        {
            Insist.RegisterException(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// <inheritdoc cref="IUpdateOperation{TKey,TUserKey,TEntity}.UpdateAsync"/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            if (UnitOfWork == null)
            {
                Context.Set<TEntity>().Update(entity);
                Context.Entry<TEntity>(entity).Property("CreatedAt").IsModified = false;
                return await Context.SaveChangesAsync(cancellationToken) > 0;
            }

            return await UnitOfWork.ExecuteAsync<bool>(() =>
            {
                Context.Set<TEntity>().Update(entity);
                Context.Entry<TEntity>(entity).Property("CreatedAt").IsModified = false;
                return Task.FromResult(true);
            }, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            Insist.RegisterException(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteAsync(System.Linq.Expressions.Expression{System.Func{TEntity,bool}},bool,System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool softDelete = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var entries = Context.Set<TEntity>().Where(predicate).ToList();
            foreach (var entry in entries)
            {
                if (!(await DeleteAsync(entry.Id, softDelete, cancellationToken)))
                    return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Insist.RegisterException(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteAsync(TKey,bool,System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(TKey id, bool softDelete = true, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await Context.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id != null && x.Id.Equals(id), cancellationToken);
            if (entity == null)
                throw new ApplicationException($"Element with Id {id} was not found");

            if (UnitOfWork == null)
            {
                if(!softDelete)
                {
                    Context.Set<TEntity>().Remove(entity);
                    return await Context.SaveChangesAsync(cancellationToken) > 0;
                }
                else
                {
                    entity.Status = false;
                    return await UpdateAsync(entity, cancellationToken);
                }
            }

            return await UnitOfWork.ExecuteAsync<bool>(() =>
            {
                Context.Set<TEntity>().Remove(entity);
                return Task.FromResult(true);
            }, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            Insist.RegisterException(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteAsync(TEntity,bool,System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> DeleteAsync(TEntity entity, bool softDelete = true, CancellationToken cancellationToken = default)
        => await DeleteAsync(entity.Id, softDelete, cancellationToken);

    /// <summary>
    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteRangeAsync(System.Collections.Generic.List{TEntity},bool,System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteRangeAsync(List<TEntity> entities, bool softDelete = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var entity in entities)
            {
                if (!(await DeleteAsync(entity, softDelete, cancellationToken)))
                    return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Insist.RegisterException(e, e.Message);
            throw;
        }
    }
    
    /// <summary>
    /// <inheritdoc cref="IDeleteOperation{TKey,TUserKey,TEntity}.DeleteRangeAsync(System.Collections.Generic.List{TKey},bool,System.Threading.CancellationToken)"/>
    /// </summary>
    /// <param name="entitiesIds"></param>
    /// <param name="softDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> DeleteRangeAsync(List<TKey> entitiesIds, bool softDelete = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var id in entitiesIds)
            {
                if (!(await DeleteAsync(id, softDelete, cancellationToken)))
                    return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Insist.RegisterException(e, e.Message);
            throw;
        }
    }
}