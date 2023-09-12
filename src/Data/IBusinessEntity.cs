namespace Oluso.Data;

/// <summary>
/// Base for all entities in the system
/// </summary>
public interface IBusinessEntity
{
}

/// <summary>
/// <inheritdoc cref="IBusinessEntity"/> and include some auditable members.
/// This base class defines the Key and Creator user data types, to make
/// more flexible the system modules
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TUserKey"></typeparam>
public interface IBusinessEntity<TKey, TUserKey> : IBusinessEntity
{
    /// <summary>
    /// Id if the entity
    /// </summary>
    TKey Id { get; set; }
    
    /// <summary>
    /// status of the entity. this field is used to mark the entity when using soft deleted
    /// mechanism, so if false system will not show it on any query result.
    /// </summary>
    bool Status { get; set; }
    
    /// <summary>
    /// creation date
    /// </summary>
    DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// how create the entity
    /// </summary>
    TUserKey? CreatedBy { get; set; }
    
    /// <summary>
    /// update date
    /// </summary>
    DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// user id of user that update the entity
    /// </summary>
    TUserKey? UpdatedBy { get; set; }
    
    /// <summary>
    /// deletion date. ths field is used only if a soft delete was applied of course 
    /// </summary>
    DateTime? DeletedAt { get; set; }
    
    
    /// <summary>
    /// deleted by, this field is used only if a soft deleted was applied of course
    /// </summary>
    TUserKey? DeletedBy { get; set; }
    
    /// <summary>
    /// unique identifier to avoid entities conflicts 
    /// </summary>
    string RowVersion { get; set; }
}