namespace Tests.Fakes;

public partial class FakeEntity
{
    public int Id { get; set; }
    
    public string Value { get; set; }

    public FakeTypes.ValueTypes.EnumerationTypes.Gender Gender { get; set; } =
        FakeTypes.ValueTypes.EnumerationTypes.Gender.NotSpecified;

    public int Age { get; set; } = 19;
}