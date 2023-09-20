using Data.Data;
using Data.Models;
using Oluso.Data;
using Oluso.Data.Repositories;

namespace Data.Repositories;

public interface ITeacherRepository : IRepository<int, string, Teacher, ApplicationDbContext>
{
}

// you could implement any specific data operation you like in here
public class TeacherRepository : Repository<int, string, Teacher, ApplicationDbContext>, ITeacherRepository
{
    // IUnitOfWork from this project not from Oluso.Trojes.IUnitOfWork<T>
    public TeacherRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}