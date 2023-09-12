using System.Linq.Expressions;
using Oluso.Data.Specifications;

namespace Tests.Fakes;

public record FakeCompilableSpecificationAgeUnder18 : CompilableSpecification<FakeEntity>
{
    public override Expression<Func<FakeEntity, bool>> ToExpression() => ent =>
        ent.Age < 18;
}

public record FakeCompilableSpecificationIsMale : CompilableSpecification<FakeEntity>
{
    public override Expression<Func<FakeEntity, bool>> ToExpression() => ent =>
        ent.Gender == FakeTypes.ValueTypes.EnumerationTypes.Gender.Male;
}

public record FakeCompilableSpecificationValueIsNullEmptyOrWhiteSpace : CompilableSpecification<FakeEntity>
{
    public override Expression<Func<FakeEntity, bool>> ToExpression() => ent =>
        string.IsNullOrEmpty(ent.Value) || string.IsNullOrWhiteSpace(ent.Value);
}
