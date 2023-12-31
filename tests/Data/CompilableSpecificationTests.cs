using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Data.Extensions;
using Shouldly;
using Tests.Fakes;

namespace Tests.Data;

public class CompilableSpecificationTests
{
    [Fact]
    public void CompilableSpecificationTest_And()
    {
        var ctx = BuildContext("And");
        ctx = Populate(ctx);
        ctx.FakeEntities.Count().ShouldBe(2);
        var under18 = new FakeCompilableSpecificationAgeUnder18();
        var isMale = new FakeCompilableSpecificationIsMale();
        var result = ctx.FakeEntities.Where(under18.And(isMale));

        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
    }
    
    [Fact]
    public void CompilableSpecificationTest_Or()
    {
        var ctx = BuildContext("Or");
        ctx = Populate(ctx);
        var under18 = new FakeCompilableSpecificationAgeUnder18();
        var isMale = new FakeCompilableSpecificationIsMale();
        var result = ctx.FakeEntities.Where(under18.Or(isMale));

        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
    }
    
    [Fact]
    public void CompilableSpecificationTest_OrOperator()
    {
        var ctx = BuildContext("OrOperator");
        ctx = Populate(ctx);
        var under18 = new FakeCompilableSpecificationAgeUnder18();
        var isMale = new FakeCompilableSpecificationIsMale();
        var result = ctx.FakeEntities.Where(under18 | isMale);

        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
    }
    
    
    [Fact]
    public void CompilableSpecificationTest_Not()
    {
        var ctx = BuildContext("Not");
        ctx = Populate(ctx);
        var under18 = new FakeCompilableSpecificationAgeUnder18();
        var result = ctx.FakeEntities.Where(under18.Not());

        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
    }
    
    
    
    private FakeDbContext BuildContext(string section)
    {
        var provider = new ServiceCollection()
            .AddDbContext<FakeDbContext>(cfg => 
                cfg.UseInMemoryDatabase($"compilableSpecification{section}TestDatabase"))
            .BuildServiceProvider();
        return provider.GetRequiredService<FakeDbContext>();
    }

    public FakeDbContext Populate(FakeDbContext context)
    {
        context.FakeEntities.AddRange(new FakeEntity[]
        {
            new() { Id = 1, Value = "Michael", Age = 12, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Male },
            new() { Id = 2, Value = "Amanda", Age = 19, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Female },
        });
        context.SaveChanges();
        return context;
    }
}