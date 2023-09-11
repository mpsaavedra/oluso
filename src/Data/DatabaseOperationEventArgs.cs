using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Oluso.Data;

/// <summary>
/// represent the data operation basic args
/// </summary>
public class DatabaseOperationEventArgs
{
    /// <summary>
    /// state represent the operation
    /// </summary>
    public EntityState State { get; set;  }

    /// <summary>
    /// entries affected by operation
    /// </summary>
    public IEnumerable<EntityEntry>? Entries { get; set; } = null;

    /// <summary>
    /// returns an empty instance of <see cref="DatabaseOperationEventArgs"/>
    /// </summary>
    public static DatabaseOperationEventArgs Empty => new DatabaseOperationEventArgs();

    /// <summary>
    /// returns a new <see cref="DatabaseOperationEventArgs"/> instance
    /// </summary>
    /// <param name="entries"></param>
    /// <returns></returns>
    public static DatabaseOperationEventArgs New(IEnumerable<EntityEntry> entries) =>
        new DatabaseOperationEventArgs() { Entries = entries };
}