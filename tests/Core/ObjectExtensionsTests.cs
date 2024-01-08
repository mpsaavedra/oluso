using Microsoft.Extensions.DependencyInjection;
using Oluso.Extensions;
using Shouldly;
using Tests.Fakes;

namespace Tests.Core;

public class ObjectExtensionsTests
{
    [Fact]
    public void PopulateWithMappedData()
    {
        var provider = BuildServiceProvider();
        var mapper = provider.GetRequiredService<AutoMapper.IMapper>();
        var req = new FakeBusinessEntityRequestView
        {
            Age = 44,
            Value = "FakeBusinessEntity1"
        };
        FakeBusinessEntity mapped = new();
        
        mapped.PopulateWithMappedData(req);
        
        mapped.Age.ShouldBe(44);
        mapped.Value.ShouldBe("FakeBusinessEntity1");

    }

    private static IServiceProvider BuildServiceProvider() =>
        new ServiceCollection()
            .AddAutoMapper(typeof(FakeAutoMapping))
            .BuildServiceProvider();
}