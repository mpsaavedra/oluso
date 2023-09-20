namespace Data.Data;

public interface IUnitOfWork : Oluso.Data.IUnitOfWork<ApplicationDbContext>
{
}

// this class name is possible to make easier to remember ;D
public class UnitOfWork : Oluso.Data.UnitOfWork<ApplicationDbContext>, IUnitOfWork
{
    public UnitOfWork(IServiceProvider provider, ApplicationDbContext context) : base(provider, context)
    {
    }
}