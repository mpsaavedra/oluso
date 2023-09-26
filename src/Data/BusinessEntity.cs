using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Oluso.Data;

/// <summary>
/// <inheritdoc cref="IBusinessEntity"/> it implements some basic
/// equality comparators and other simple operators
/// </summary>
public class BusinessEntity<TKey, TUserKey> : IBusinessEntity<TKey, TUserKey>, IBusinessEntity, IEquatable<object>
{
    /// <summary>
    /// returns a new <see cref="BusinessEntity{TKey,TUserKey}"/> instance
    /// </summary>
#pragma warning disable CS8618
    public BusinessEntity()
#pragma warning restore CS8618
    {
        CreatedAt = DateTime.UtcNow;
        RowVersion = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// returns a new <see cref="BusinessEntity{TKey,TUserKey}"/> instance
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <param name="createAt"></param>
    /// <param name="createdBy"></param>
    /// <param name="updatedAt"></param>
    /// <param name="updatedBy"></param>
    /// <param name="deletedAt"></param>
    /// <param name="deletedBy"></param>
    /// <param name="rowVersion"></param>
    public BusinessEntity(TKey id, bool status, DateTime createAt, TUserKey? createdBy, DateTime? updatedAt,
        TUserKey? updatedBy, DateTime? deletedAt, TUserKey? deletedBy, string rowVersion)
    {
        Id = id;
        Status = status;
        CreatedAt = createAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;
        RowVersion = rowVersion;
    }

    /// <summary>
    /// returns a new <see cref="BusinessEntity{TKey,TUserKey}"/> instance
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <param name="createdBy"></param>
    public BusinessEntity(TKey id, bool status, TUserKey? createdBy)
    {
        Id = id;
        Status = status;
        CreatedBy = createdBy;
        RowVersion = Guid.NewGuid().ToString();
        CreatedAt = DateTime.UtcNow;
    }

    /// <inheritdoc cref="IBusinessEntity{TKey, TUserKey}.Id"/>
    [Key]
    [JsonPropertyName("id")]
    public TKey Id { get; set; }

    /// <inheritdoc cref="IBusinessEntity{TKey,TUserKey}.Status"/>
    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;

    /// <inheritdoc cref="IBusinessEntity{TKey,TUserKey}.CreatedAt"/>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <inheritdoc cref="IBusinessEntity{TKey,TUserKey}.CreatedBy"/>
    [JsonPropertyName("createdBy")]
    public TUserKey? CreatedBy { get; set; }

    /// <inheritdoc cref="IBusinessEntity{TKey,TUserKey}.UpdatedAt"/>
    [JsonPropertyName("updateAt")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// <inheritdoc cref="IBusinessEntity{TKey,TUserKey}.UpdatedBy"/>
    /// </summary>
    [JsonPropertyName("updatedBy")]
    public TUserKey? UpdatedBy { get; set; }

    /// <inheritdoc cref="IBusinessEntity{TKey, TUserKey}.DeletedAt"/>
    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; }

    /// <inheritdoc cref="IBusinessEntity{TKey,TUserKey}.DeletedBy"/>
    [JsonPropertyName("deletedBy")]
    public TUserKey? DeletedBy { get; set; }

    /// <inheritdoc cref="IBusinessEntity{TKey,TUserKey}.RowVersion"/>
    [JsonPropertyName("rowVersion")]
    public string RowVersion { get; set; } = Guid.NewGuid().ToString();
    
    #region Comparisons

    /// <inheritdoc cref="IEquatable{T}"/>
    public override bool Equals(object? obj)
    {
        var compareTo = obj as BusinessEntity<TKey, TUserKey>;

        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;

        return Id != null && Id.Equals(compareTo.Id);
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator ==( BusinessEntity<TKey, TUserKey> a,  BusinessEntity<TKey, TUserKey> b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator !=( BusinessEntity<TKey, TUserKey> a,  BusinessEntity<TKey, TUserKey> b)
    {
        return !Equals(a, b);
    }

    /// <summary>
    /// <inheritdoc cref="object.GetHashCode"/>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 907) + (Id != null ? Id.GetHashCode() : 314);
    }

    /// <summary>
    /// <inheritdoc cref="object.ToString"/>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}, RowVersion={RowVersion}]";
    }

    #endregion
}