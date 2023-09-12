using Oluso.Data;

namespace Tests.Fakes;

public interface IFakeUnitOfWork : IUnitOfWork<FakeDbContextBaseDbContext>
{
}

public class FakeUnitOfWork : UnitOfWork<FakeDbContextBaseDbContext>, IFakeUnitOfWork
{
    public FakeUnitOfWork(IServiceProvider provider, FakeDbContextBaseDbContext context) : base(provider, context)
    {
    }
}