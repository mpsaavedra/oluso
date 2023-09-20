using Data.Models;
using Oluso.Data.Specifications;

namespace Data.Repositories.Specifications;

public record StudentIsMale : Specification<Student>
{
    public override Func<Student, bool> ToFunc() => x => x.Gender == Gender.Male;
}

public record StudentIsUnder21 : Specification<Student>
{
    public override Func<Student, bool> ToFunc() => x => x.Age < 21;
}