using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oluso.Data;
using Oluso.Data.Extensions;

namespace Tests.Fakes;

// only value because BusinessEntity already has auditory data on it
public class FakeBusinessEntity : BusinessEntity<int, string>
{
#pragma warning disable CS8618
    public string Value { get; set; }
#pragma warning restore CS8618

    public FakeTypes.ValueTypes.EnumerationTypes.Gender Gender { get; set; } =
        FakeTypes.ValueTypes.EnumerationTypes.Gender.NotSpecified;

    public int Age { get; set; } = 19;
    public int FakeBusinessEntity2Id { get; set; }
#pragma warning disable CS8618 
    public FakeBusinessEntity2 FakeBusinessEntity2 { get; set; }
#pragma warning restore CS8618 
}

public class FakeBusinessEntity2 : BusinessEntity<int, string>
{
#pragma warning disable CS8618
    public string Dni { get; set; }
#pragma warning restore CS8618
    
    public int FakeBusinessEntityId { get; set; }
#pragma warning disable CS8618
    public FakeBusinessEntity FakeBusinessEntity { get; set; }
#pragma warning restore CS8618
}

public class FakeBusinessEntityTypeConfiguration : IEntityTypeConfiguration<FakeBusinessEntity>
{
    public void Configure(EntityTypeBuilder<FakeBusinessEntity> builder)
    {
        // apply base BusinessEntity configuration
        builder.ConfigureBusinessEntity<int, string, FakeBusinessEntity>();
        builder.HasOne(x => x.FakeBusinessEntity2)
            .WithOne(x => x.FakeBusinessEntity);
    }
}

public class FakeBusinessEntity2TypeConfiguration : IEntityTypeConfiguration<FakeBusinessEntity2>
{
    public void Configure(EntityTypeBuilder<FakeBusinessEntity2> builder)
    {
        // apply base BusinessEntity configuration
        builder.ConfigureBusinessEntity<int, string, FakeBusinessEntity2>();
        builder.HasOne(x => x.FakeBusinessEntity)
            .WithOne(x => x.FakeBusinessEntity2)
            .HasForeignKey<FakeBusinessEntity2>(x => x.FakeBusinessEntityId);
    }
}