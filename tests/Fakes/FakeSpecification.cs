using System.Linq.Expressions;
using Oluso.Data.Specifications;

namespace Tests.Fakes;


public record FakeSpecificationAgeUnder18 : Specification<FakeEntity>
{
    public override Func<FakeEntity, bool> ToFunc() => ent =>
        ent.Age < 18;
}

public record FakeSpecificationIsMale : Specification<FakeEntity>
{
    public override Func<FakeEntity, bool> ToFunc() => ent =>
        ent.Gender == FakeTypes.ValueTypes.EnumerationTypes.Gender.Male;
}

public record FakeSpecificationValueIsNullEmptyOrWhiteSpace : Specification<FakeEntity>
{
    public override Func<FakeEntity, bool> ToFunc() => ent =>
        string.IsNullOrEmpty(ent.Value) || string.IsNullOrWhiteSpace(ent.Value);
}