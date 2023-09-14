using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Data.Extensions;
using Shouldly;
using Tests.Fakes;

namespace Tests.Data;

public class RepositoryTests
{
    #region Query Repository Tests

    [Fact]
    public async Task QueryRepositoryTest_FindAsync_Predicate()
    {
        var repo = BuildRepository("FindAsync_Predicate");
        (await repo.FindAsync(x => x.Age >= 17 && x.Age <= 19)).Count().ShouldBe(6);
        (await repo.FindAsync(x => x.Age == 20 )).Count().ShouldBe(4);
    }

    [Fact]
    public async Task QueryRepositoryTest_FindPaginatedAsync_Predicate()
    {
        var repo = BuildRepository("FindPaginatedAsync_Predicate");
        var result = await repo.FindPaginatedAsync(x => x.Age > 2, pageSize: 2);
        result.CurrentPage.ShouldBe(0);
        result.Descriptor.NumberOfPages.ShouldBe(5);
        result.Descriptor.PageBoundaries.Length.ShouldBe(5);
    }

    [Fact]
    public async Task QueryRepositoryTest_FindAsync_Specification()
    {
        var repo = BuildRepository("FindAsync_Specification");
        var isFemale = new FakeSpecificationIsFemale();
        var is19 = new FakeSpecification19Years();
        (await repo.FindAsync(isFemale)).Count().ShouldBe(5);
        (await repo.FindAsync(isFemale.And(is19))).Count().ShouldBe(1);
        (await repo.FindAsync(isFemale.And(is19.Not()))).Count().ShouldBe(4);
    }

    [Fact]
    public async Task QueryRepositoryTest_FindPaginatedAsync_Specification()
    {
        var repo = BuildRepository("FindPaginatedAsync_Specification");
        var isTeenager = new FakeSpecificationTeenAger();
        var result = await repo.FindPaginatedAsync(isTeenager, pageSize: 2);
        result.CurrentPage.ShouldBe(0);
        result.Descriptor.NumberOfPages.ShouldBe(2);
        result.Descriptor.PageBoundaries.Length.ShouldBe(2);
    }
    
    #endregion

    #region Command Repository

    [Fact]
    public async Task CommandRepositoryTests_Create()
    {
        var repo = BuildRepository("Create");
        var id = await repo.CreateAsync(new FakeBusinessEntity { Id = 11, Value = "Newly added" })!;
        id.ShouldBePositive();
        (await repo.CountAsync()).ShouldBe(11);
        var entity = await repo.GetEntity<FakeBusinessEntity>().FirstOrDefaultAsync(x => x.Id == 11);
        entity.ShouldNotBeNull();
        entity.Value.ShouldBe("Newly added");
    }

    [Fact]
    public async Task CommandRepositoryTests_CreateRange()
    {
        var repo = BuildRepository("CreateRange");
        var ids = await repo.CreateRangeAsync(new List<FakeBusinessEntity>()
        {
            new () { Id = 11, Value = "Newly added" },
            new () { Id = 12, Value = "Newly added 2" },
        })!;
        ids.Count().ShouldBe(2);
        (await repo.CountAsync()).ShouldBe(12);
        var entity = await repo.GetEntity<FakeBusinessEntity>().FirstOrDefaultAsync(x => x.Id == 11);
        entity.ShouldNotBeNull();
        entity.Value.ShouldBe("Newly added");
    }

    [Fact]
    public async Task CommandRepositoryTests_Update()
    {
        var repo = BuildRepository("Update");
        var id = await repo.CreateAsync(new FakeBusinessEntity { Id = 11, Value = "Newly added" })!;
        id.ShouldBePositive();
        (await repo.CountAsync()).ShouldBe(11);
        var entity = await repo.GetEntity<FakeBusinessEntity>().FirstOrDefaultAsync(x => x.Id == 11);
        entity.ShouldNotBeNull();
        entity.Value.ShouldBe("Newly added");
        entity.Value = "Modified";
        await repo.UpdateAsync(entity);
        entity = await repo.GetEntity<FakeBusinessEntity>().FirstOrDefaultAsync(x => x.Id == 11);
        entity.ShouldNotBeNull();
        entity.Value.ShouldBe("Modified");
    }

    [Fact]
    public async Task CommandRepositoryTests_DeleteById()
    {
        var repo = BuildRepository("DeleteById");
        await repo.DeleteAsync(10);
        (await repo.CountAsync()).ShouldBe(9);
    }

    [Fact]
    public async Task CommandRepositoryTests_DeleteByPredicate()
    {
        var repo = BuildRepository("DeleteByPredicate");
        await repo.DeleteAsync(x => x.Id == 10);
        (await repo.CountAsync()).ShouldBe(9);
    }

    [Fact]
    public async Task CommandRepositoryTests_DeleteByEntity()
    {
        var repo = BuildRepository("DeleteByEntity");
        var entity = await repo.GetEntity<FakeBusinessEntity>().FirstOrDefaultAsync(x => x.Id == 10);
        await repo.DeleteAsync(entity!);
        (await repo.CountAsync()).ShouldBe(9);
    }

    [Fact]
    public async Task CommandRepositoryTests_DeleteRangeById()
    {
        var repo = BuildRepository("DeleteRangeById");
        await repo.DeleteRangeAsync(new List<int>() { 1, 2, 3 });
        (await repo.CountAsync()).ShouldBe(7);
    }

    [Fact]
    public async Task CommandRepositoryTests_DeleteRangeByEntity()
    {
        var repo = BuildRepository("DeleteRangeByEntity");
        var entity = await repo.GetEntity<FakeBusinessEntity>().FirstOrDefaultAsync(x => x.Id == 10);
        await repo.DeleteRangeAsync(new List<FakeBusinessEntity>() { entity! });
        (await repo.CountAsync()).ShouldBe(9);
    }

    #endregion
    
    private static IFakeRepository BuildRepository(string section)
    {
        var provider = new ServiceCollection()
            .AddDbContext<FakeDbContextBaseDbContext>(cfg =>
                cfg.UseInMemoryDatabase($"repository{section}TestDatabase"))
            .AddSingleton<IFakeUnitOfWork, FakeUnitOfWork>()
            .AddScoped<IFakeRepository, FakeRepository>()
            .BuildServiceProvider();
        Populate(provider.GetRequiredService<FakeDbContextBaseDbContext>());
        return provider.GetRequiredService<IFakeRepository>();
    }

    private static void Populate(FakeDbContextBaseDbContext ctx)
    {
        ctx.FakeBusinessEntities.AddRange(new FakeBusinessEntity[]
        {
            new () { Id = 1,  Value = "Entity 1",  Age = 17, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Male},
            new () { Id = 2,  Value = "Entity 3",  Age = 17, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Male },
            new () { Id = 3,  Value = "Entity 3",  Age = 17, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Female },
            new () { Id = 4,  Value = "Entity 4",  Age = 17, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Female },
            new () { Id = 5,  Value = "Entity 5",  Age = 19, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Male },
            new () { Id = 6,  Value = "Entity 6",  Age = 19, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Female },
            new () { Id = 7,  Value = "Entity 7",  Age = 20, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Female },
            new () { Id = 8,  Value = "Entity 8",  Age = 20, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Female },
            new () { Id = 9,  Value = "Entity 9",  Age = 20, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Male },
            new () { Id = 10, Value = "Entity 10", Age = 20, Gender = FakeTypes.ValueTypes.EnumerationTypes.Gender.Male },
        });
        ctx.SaveChanges();
    }
}