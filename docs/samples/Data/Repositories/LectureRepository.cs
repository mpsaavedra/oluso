using Data.Data;
using Data.Models;
using Oluso.Data;
using Oluso.Data.Repositories;

namespace Data.Repositories;

public interface ILectureRepository : IRepository<int, string, Lecture, ApplicationDbContext>
{
}

// you could implement any specific data operation you like in here
public class LectureRepository : Repository<int, string, Lecture, ApplicationDbContext>, ILectureRepository
{
    // IUnitOfWork from this project not from Oluso.Trojes.IUnitOfWork<T>
    public LectureRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}