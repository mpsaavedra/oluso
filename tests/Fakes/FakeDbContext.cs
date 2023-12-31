using Microsoft.EntityFrameworkCore;
using Oluso.Data;

namespace Tests.Fakes;

public class FakeDbContext : DbContext
{
    public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options)
    {
        
    }

    public virtual DbSet<FakeEntity> FakeEntities => Set<FakeEntity>();
}


public class FakeDbContextBaseDbContext : BaseDbContext
{
    public FakeDbContextBaseDbContext(DbContextOptions<FakeDbContextBaseDbContext> options) : base(options)
    {
        
    }

    public virtual DbSet<FakeBusinessEntity> FakeBusinessEntities => Set<FakeBusinessEntity>();
    
    public virtual DbSet<FakeBusinessEntity2> FakeBusinessEntities2 => Set<FakeBusinessEntity2>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FakeDbContext).Assembly);
    }
}