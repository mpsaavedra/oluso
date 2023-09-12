using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Oluso.Data.Extensions;

/// <summary>
/// DbContext related extensions
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Configure the <see cref="BusinessEntity{TKey,TUserKey}"/> properties
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="maxLengthUser"></param>
    /// <param name="generatedKeyValue"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static EntityTypeBuilder<TEntity> ConfigureBusinessEntity<TKey, TUserKey, TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        int maxLengthUser = 256,
        bool generatedKeyValue = true)
        where TEntity : class, IBusinessEntity<TKey, TUserKey>
    {
        if (generatedKeyValue)
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        else
            builder.Property(x => x.Id).ValueGeneratedNever();
        
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).HasMaxLength(maxLengthUser).IsRequired(false);
        builder.Property(x => x.DeletedAt).IsRequired(false);
        builder.Property(x => x.DeletedBy).HasMaxLength(maxLengthUser).IsRequired(false);
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.UpdatedBy).HasMaxLength(maxLengthUser).IsRequired(false);
        builder.Property(x => x.RowVersion).IsRequired();
        builder.HasIndex(x => x.Id).IsUnique();
        builder.HasIndex(x => x.RowVersion).IsUnique();
        return builder;
    }
}