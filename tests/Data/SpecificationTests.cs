using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Data.Extensions;
using Shouldly;
using Tests.Fakes;

namespace Tests.Data;

public class SpecificationTests
{
    [Fact]
    public void SpecificationTest_And()
    {
        var ctx = BuildContext("And");
        ctx.FakeEntities.Count().ShouldBe(2);
        var under18 = new FakeSpecificationAgeUnder18();
        var isMale = new FakeSpecificationIsMale();
        var result = ctx.FakeEntities.Where(under18.And(isMale));

        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
    }
    
    [Fact]
    public void SpecificationTest_Or()
    {
        var ctx = BuildContext("Or");
        var under18 = new FakeSpecificationAgeUnder18();
        var isMale = new FakeSpecificationIsMale();
        var result = ctx.FakeEntities.Where(under18.Or(isMale));

        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
    }
    
    [Fact]
    public void SpecificationTest_OrOperator()
    {
        var ctx = BuildContext("OrOperator");
        var under18 = new FakeSpecificationAgeUnder18();
        var isMale = new FakeSpecificationIsMale();
        var result = ctx.FakeEntities.Where(under18 | isMale);

        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
    }
    
    
    [Fact]
    public void SpecificationTest_Not()
    {
        var ctx = BuildContext("Not");
        var under18 = new FakeSpecificationAgeUnder18();
        var result = ctx.FakeEntities.Where(under18.Not());

        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
    }
    
    private FakeDbContext BuildContext(string section)
    {
        var provider = new ServiceCollection()
            .AddDbContext<FakeDbContext>(cfg => 
                cfg.UseInMemoryDatabase($"specification{section}TestDatabase"))
            .BuildServiceProvider();
        var ctx = provider.GetRequiredService<FakeDbContext>();
        Populate(ctx);
        return provider.GetRequiredService<FakeDbContext>();
    }

    private void Populate(FakeDbContext context)
    {
        context.FakeEntities.AddRange(new FakeEntity[]
        {
            new() { Id = 1, Value = "Michael", Age = 12, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Male },
            new() { Id = 2, Value = "Amanda", Age = 19, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Female },
        });
        context.SaveChanges();
    }
}