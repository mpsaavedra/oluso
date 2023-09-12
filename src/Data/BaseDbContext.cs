using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Oluso.Authentication;

namespace Oluso.Data;

/// <summary>
/// Base <see cref="DbContext"/> used in the system (microservices and all) defines the function
/// <see cref="BaseDbContext{TUserKey}.SaveEntitiesChangesAsync(CancellationToken)"/>. This class is used when
/// no auditable information is required. It include events that could be used to perform some action with a given operation is
/// launch -event are only oriented to data modification- every event receive those entities that
/// are affected by the action. <b>events are called after execute the action and before save changes to database</b>
/// </summary>
public class BaseDbContext : BaseDbContext<string>
{
    /// <summary>
    /// returns a new <see cref="BaseDbContext"/> instance
    /// </summary>
    /// <param name="authenticatedUser"></param>
    /// <param name="options"></param>
    public BaseDbContext(IAuthenticatedUser<string> authenticatedUser, DbContextOptions options) : base(authenticatedUser, options)
    {
    }

    /// <summary>
    /// returns a new <see cref="BaseDbContext"/> instance.
    /// </summary>
    /// <param name="options"></param>
    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }
    
    /// <summary>
    /// delegate to notify operations in the database events 
    /// </summary>
    public delegate void DatabaseOperationEventHandler(object? sender, DatabaseOperationEventArgs args);

    /// <summary>
    /// delegate to notify operations in the database events 
    /// </summary>
    /// <typeparam name="TDatabaseOperationEventArgs"></typeparam>
    public delegate void DatabaseOperationEventHandler<in TDatabaseOperationEventArgs>(object? sender,
        TDatabaseOperationEventArgs args);
}

/// <summary>
/// Base <see cref="DbContext"/> used in the system (microservices and all) defines the function
/// <see cref="BaseDbContext{TUserKey}.SaveEntitiesChangesAsync(CancellationToken)"/> that update the auditable information
/// of the entities. It include events that could be used to perform some action with a given operation is
/// launch -event are only oriented to data modification- every event receive those entities that
/// are affected by the action. <b>events are called after execute the action and before save changes to database</b>
/// </summary> 
public class BaseDbContext<TUserKey> : DbContext 
    where TUserKey : class
{
    /// <summary>
    /// Currently authenticated user if authentication used
    /// </summary>
    private readonly IAuthenticatedUser<TUserKey>? _authenticatedUser;
    
    /// <summary>
    /// User Id user to seed data
    /// </summary>
    // ReSharper disable once StaticMemberInGenericType
    private static readonly Guid UserSeedId = Guid.Parse("2E9D5237-4453-43C6-B6E3-F31207E25241");

    /// <summary>
    /// event to invoke when entities where added
    /// </summary>
    public event BaseDbContext.DatabaseOperationEventHandler? OnEntitiesAdded;

    /// <summary>
    /// event to invoke when entities where modified
    /// </summary>
    public event BaseDbContext.DatabaseOperationEventHandler? OnEntitiesUpdated;

    /// <summary>
    /// event to invoke when entities where deleted
    /// </summary>
    public event BaseDbContext.DatabaseOperationEventHandler? OnEntitiesDeleted;

    /// <summary>
    /// returns a new <see cref="BaseDbContext{TUserKey}"/> instance 
    /// </summary>
    /// <param name="authenticatedUser"></param>
    /// <param name="options"></param>
    public BaseDbContext(
        IAuthenticatedUser<TUserKey> authenticatedUser,
        DbContextOptions options) : base(options)
    {
        _authenticatedUser = authenticatedUser;
    }
    
    /// <summary>
    /// returns a new <see cref="BaseDbContext{TUserKey}"/> instance 
    /// </summary>
    /// <param name="options"></param>
    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    /// Saves data into the database and updates the auditable information of affected entities.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public virtual async Task<int> SaveEntitiesChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries().Where(x => x.Entity is IBusinessEntity);
        {
            var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
            var added = entityEntries.Where(e => e.State == EntityState.Added);
            var modified = entityEntries.Where(e => e.State == EntityState.Modified);
            var deleted = entityEntries.Where(e => e.State == EntityState.Deleted);

            if(added.Any()) ProcessAddedEntities(added);
            if(modified.Any()) ProcessModifiedEntities(modified);
            if(deleted.Any()) ProcessDeletedEntities(deleted);
        }


        var result = await SaveChangesAsync(cancellationToken);
        return result;
    }

    /// <summary>
    /// Saves data into the database and while doing it it updates the auditable information of the
    /// entities.
    /// </summary>
    /// <returns></returns>
    public virtual int SaveEntitiesChanges() => SaveEntitiesChangesAsync().Result;

    private void ProcessAddedEntities(IEnumerable<EntityEntry> entries)
    {
        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            var userId = GetUserId(entry);
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.GetType().GetProperty("CreatedAt")?.SetValue(entry.Entity, DateTime.UtcNow);
                    entry.Entity.GetType().GetProperty("CreatedBy")?.SetValue(entry.Entity, userId);
                    entry.Entity.GetType().GetProperty("RowVersion")?.SetValue(entry.Entity, Guid.NewGuid().ToString());
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                case EntityState.Modified:
                default:
                    throw new ApplicationException($"Entry {entry} is not Added state");
            }
        }
        OnEntitiesAdded?.Invoke(this, DatabaseOperationEventArgs.New(entityEntries));
    }

    private void ProcessModifiedEntities(IEnumerable<EntityEntry> entries)
    {
        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            var userId = GetUserId(entry);
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.Entity.GetType().GetProperty("UpdatedAt")?.SetValue(entry.Entity, DateTime.UtcNow);
                    entry.Entity.GetType().GetProperty("UpdatedBy")?.SetValue(entry.Entity, userId);
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                case EntityState.Added:
                default:
                    throw new ApplicationException($"Entry {entry} is not Modified state");
            }
        }
        OnEntitiesUpdated?.Invoke(this, DatabaseOperationEventArgs.New(entityEntries));
    }

    private void ProcessDeletedEntities(IEnumerable<EntityEntry> entries)
    {
        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            var userId = GetUserId(entry);
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.Entity.GetType().GetProperty("DeletedAt")?.SetValue(entry.Entity, DateTime.UtcNow);
                    entry.Entity.GetType().GetProperty("DeletedBy")?.SetValue(entry.Entity, userId);
                    entry.Entity.GetType().GetProperty("State")?.SetValue(entry.Entity, false);
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Modified:
                case EntityState.Added:
                default:
                    throw new ApplicationException($"Entry {entry} is not Deleted state");
            }
        }
        OnEntitiesDeleted?.Invoke(this, DatabaseOperationEventArgs.New(entityEntries));
    }

    private TUserKey GetUserId(EntityEntry entry)
    {
        var userId = _authenticatedUser?.UserId;

        if (userId == null || string.IsNullOrWhiteSpace(userId.ToString()))
        {
            userId = UserSeedId.ToString() as TUserKey;
        }

        return userId!;
    }
        
    
}