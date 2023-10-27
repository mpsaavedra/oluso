using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Tests.Fakes;

namespace Tests.Data;

public class UnitOfWorkTests
{
    [Fact]
    public async Task UnitOfWorkTests_Add()
    {
        var unitOfWork = BuildContext("unitOfWork{section}TestDatabase_Add");
        await unitOfWork.ExecuteAsync(() =>
        {
            unitOfWork.Context.FakeBusinessEntities.Add(new FakeBusinessEntity { Id = 1, Value = "Some value 1" });
            unitOfWork.Context.FakeBusinessEntities.Add(new FakeBusinessEntity { Id = 2, Value = "Some value 2" });
        });
        unitOfWork.Context.FakeBusinessEntities.Count().ShouldBe(2);
    }

    [Fact]
    public async Task UnitOfWorkTests_Update()
    {
        var unitOfWork = BuildContext("unitOfWork{section}TestDatabase_Update");
        await unitOfWork.ExecuteAsync(() =>
        {
            unitOfWork.Context.FakeBusinessEntities.Add(new FakeBusinessEntity { Id = 1, Value = "Some value 1" });
            unitOfWork.Context.FakeBusinessEntities.Add(new FakeBusinessEntity { Id = 2, Value = "Some value 2" });
        });
        unitOfWork.Context.FakeBusinessEntities.Count().ShouldBe(2);
        await unitOfWork.ExecuteAsync(() =>
        {
            var entity = unitOfWork.Context.FakeBusinessEntities.First(x => x.Id == 1);
            entity.Value = "Modified";
            unitOfWork.Context.Update(entity);
        });
        var entity = unitOfWork.Context.FakeBusinessEntities.First(x => x.Id == 1);
        entity.Value.ShouldBe("Modified");
    }

    [Fact]
    public async Task UnitOfWorkTests_Delete()
    {
        var unitOfWork = BuildContext("unitOfWork{section}TestDatabase_Delete");
        await unitOfWork.ExecuteAsync(() =>
        {
            unitOfWork.Context.FakeBusinessEntities.Add(new FakeBusinessEntity { Id = 1, Value = "Some value 1" });
            unitOfWork.Context.FakeBusinessEntities.Add(new FakeBusinessEntity { Id = 2, Value = "Some value 2" });
        });
        unitOfWork.Context.FakeBusinessEntities.Count().ShouldBe(2);
        await unitOfWork.ExecuteAsync(() =>
        {
            var entity = unitOfWork.Context.FakeBusinessEntities.First();
            unitOfWork.Context.Remove(entity);
        });
        unitOfWork.Context.FakeBusinessEntities.Count().ShouldBe(1);
    }

    [Fact]
    public async Task UnitOfWorkTests_DoubleOperation()
    {
        var unitOfWork = BuildUnitOfWork("DoubleOperation");
        await unitOfWork.ExecuteAsync(
            async () =>
            {
                var id = await unitOfWork.Repository<IFakeRepository2>()!
                    .CreateAsync(new FakeBusinessEntity2
                    {
                        Dni = "TEST_DNI"
                    })!;

                var ent = await unitOfWork.Repository<IFakeRepository2>()!
                    .Query.FirstOrDefaultAsync(x => x.Id == id);
                
                var id2 = unitOfWork.Repository<IFakeRepository>()!
                    .CreateAsync(new FakeBusinessEntity
                    {
                        Age = 44,
                        Value = "DEFAULT_VALUE",
                        FakeBusinessEntity2 = ent!
                    });
                    
                return true;
            });
    }
    
    private IFakeUnitOfWork BuildContext(string section)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<FakeDbContextBaseDbContext>(cfg => 
                cfg.UseInMemoryDatabase($"unitOfWork{section}TestDatabase"))
            .AddTransient<IFakeUnitOfWork, FakeUnitOfWork>()
            .BuildServiceProvider();
        return serviceProvider.GetRequiredService<IFakeUnitOfWork>();
    }
    
    private IFakeUnitOfWork BuildUnitOfWork(string section)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<FakeDbContextBaseDbContext>(cfg => 
                cfg.UseInMemoryDatabase($"unitOfWork{section}TestDatabase"))
            .AddTransient<IFakeUnitOfWork, FakeUnitOfWork>()
            .AddTransient<IFakeRepository, FakeRepository>()
            .AddTransient<IFakeRepository2, FakeRepository2>()
            .BuildServiceProvider();
        return serviceProvider.GetRequiredService<IFakeUnitOfWork>();
    }

}