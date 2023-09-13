using Oluso.Data;
using Oluso.Data.Repositories;

namespace Tests.Fakes;

public interface IFakeRepository : IRepository<int, string, FakeBusinessEntity, FakeDbContextBaseDbContext>
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