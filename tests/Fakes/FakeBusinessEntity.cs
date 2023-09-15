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
}

public class FakeBusinessEntityTypeConfiguration : IEntityTypeConfiguration<FakeBusinessEntity>
{
    public void Configure(EntityTypeBuilder<FakeBusinessEntity> builder)
    {
        // apply base BusinessEntity configuration
        builder.ConfigureBusinessEntity<int, string, FakeBusinessEntity>();
    }
}