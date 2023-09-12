using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oluso.Data;
using Oluso.Data.Extensions;

namespace Tests.Fakes;

// only value because BusinessEntity already has auditory data on it
public class FakeBusinessEntity : BusinessEntity<int, string>
{
    public string Value { get; set; }
}

public class FakeBusinessEntityTypeConfiguration : IEntityTypeConfiguration<FakeBusinessEntity>
{
    public void Configure(EntityTypeBuilder<FakeBusinessEntity> builder)
    {
        // apply base BusinessEntity configuration
        builder.ConfigureBusinessEntity<int, string, FakeBusinessEntity>();
    }
}