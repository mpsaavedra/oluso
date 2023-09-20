using Data.Data;
using Data.Models;
using Oluso.Data;
using Oluso.Data.Repositories;

namespace Data.Repositories;

public interface IStudentRepository : IRepository<int, string, Student, ApplicationDbContext>
{
}

// you could implement any specific data operation you like in here
public class StudentRepository : Repository<int, string, Student, ApplicationDbContext>, IStudentRepository
{
    // IUnitOfWork from this project not from Oluso.Trojes.IUnitOfWork<T>
    public StudentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}