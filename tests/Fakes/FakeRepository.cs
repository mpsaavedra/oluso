using Oluso.Data;
using Oluso.Data.Repositories;

namespace Tests.Fakes;

public interface IFakeRepository : IRepository<int, string, FakeBusinessEntity, FakeDbContextBaseDbContext>
{
}

public interface IFakeRepository2 : IRepository<int, string, FakeBusinessEntity2, FakeDbContextBaseDbContext>
{
}

public class FakeRepository : 
    Repository<int, string, FakeBusinessEntity, FakeDbContextBaseDbContext>,
    IFakeRepository
{
    public FakeRepository(IFakeUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}

public class FakeRepository2 : 
    Repository<int, string, FakeBusinessEntity2, FakeDbContextBaseDbContext>,
    IFakeRepository2
{
    public FakeRepository2(IFakeUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}