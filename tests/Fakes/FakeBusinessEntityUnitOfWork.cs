using Oluso.Data;

namespace Tests.Fakes;

public interface IFakeBusinessEntityUnitOfWork : IUnitOfWork<FakeDbContextBaseDbContext>
{
}

public class FakeBusinessEntityUnitOfWork : UnitOfWork<FakeDbContextBaseDbContext>, IFakeBusinessEntityUnitOfWork
{
    public FakeBusinessEntityUnitOfWork(IServiceProvider provider, FakeDbContextBaseDbContext context) : base(provider,
        context)
    {
    }
}