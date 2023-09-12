using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Oluso.Data;
using Shouldly;
// using Microsoft.EntityFrameworkCore.
using Tests.Fakes;

namespace Tests.Data;

public class BaseDbContextTests
{
    [Fact]
    public void BaseDbContextTests_OnAddedEvent()
    {
        void OnAddedEvent(object? sender, DatabaseOperationEventArgs args)
        {
            args.Entries.ShouldNotBeNull();
            args.Entries.Count().ShouldBe(2);
            args.Entries.First().Entity.ShouldBeAssignableTo(typeof(FakeBusinessEntity));
            var entity = (FakeBusinessEntity)args.Entries.First().Entity;
            entity.Value.ShouldBe("Sample value 1");
            entity.CreatedAt.ToString(CultureInfo.InvariantCulture).ShouldNotBeNullOrWhiteSpace();
        }

        var ctx = BuildContext("OnAddedEvent");
        ctx.OnEntitiesAdded += OnAddedEvent;
        
        ctx.FakeBusinessEntities.AddRange(new[]
        {
            new FakeBusinessEntity { Value = "Sample value 1" },
            new FakeBusinessEntity { Value = "Sample value 2" }
        });
        
        ctx.SaveEntitiesChanges();
    }

    [Fact]
    public void BaseDbContextTests_OnUpdatedEvent()
    {
        void OnUpdatedEvent(object? sender, DatabaseOperationEventArgs args)
        {
            args.Entries.ShouldNotBeNull();
            args.Entries.Count().ShouldBe(1); // because we only affect 1 entry
            args.Entries.First().Entity.ShouldBeAssignableTo(typeof(FakeBusinessEntity));
            var entity = (FakeBusinessEntity)args.Entries.First().Entity;
            entity.Value.ShouldBe("Modified");
            entity.UpdatedAt.ToString().ShouldNotBeNullOrWhiteSpace();
        }

        var ctx = BuildContext("OnUpdatedEvent");
        ctx.OnEntitiesUpdated += OnUpdatedEvent;
        
        ctx.FakeBusinessEntities.AddRange(new[]
        {
            new FakeBusinessEntity { Id = 1, Value = "Sample value 1" },
            new FakeBusinessEntity { Id = 2, Value = "Sample value 2" }
        });
        
        ctx.SaveEntitiesChanges();

        var entity = ctx.FakeBusinessEntities.First(x => x.Id == 1);
        entity.Value = "Modified";
        ctx.Update(entity);
        ctx.SaveEntitiesChanges();
    }

    [Fact]
    public void BaseDbContextTests_OnDeletedEvent()
    {
        void OnDeletedEvent(object? sender, DatabaseOperationEventArgs args)
        {
            args.Entries.ShouldNotBeNull();
            args.Entries.Count().ShouldBe(1);
            args.Entries.First().Entity.ShouldBeAssignableTo(typeof(FakeBusinessEntity));
        }

        var ctx = BuildContext("OnDeletedEvent");
        ctx.OnEntitiesUpdated += OnDeletedEvent;
        
        ctx.FakeBusinessEntities.AddRange(new[]
        {
            new FakeBusinessEntity { Id = 1, Value = "Sample value 1" },
            new FakeBusinessEntity { Id = 2, Value = "Sample value 2" }
        });
        
        ctx.SaveEntitiesChanges();

        var entity = ctx.FakeBusinessEntities.First(x => x.Id == 1);
        ctx.Remove(entity);
        ctx.SaveEntitiesChanges();
    }
    
    private FakeDbContextBaseDbContext BuildContext(string section)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<FakeDbContextBaseDbContext>(cfg => 
                cfg.UseInMemoryDatabase($"dbContest{section}TestDatabase"))
            .BuildServiceProvider();
        return serviceProvider.GetRequiredService<FakeDbContextBaseDbContext>();
    }
}