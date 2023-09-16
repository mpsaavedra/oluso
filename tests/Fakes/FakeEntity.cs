namespace Tests.Fakes;

public partial class FakeEntity
{
    public int Id { get; set; }
    
#pragma warning disable CS8618
    public string Value { get; set; }
#pragma warning restore CS8618

    public FakeTypes.ValueTypes.EnumerationTypes.Gender Gender { get; set; } =
        FakeTypes.ValueTypes.EnumerationTypes.Gender.NotSpecified;

    public int Age { get; set; } = 19;
}