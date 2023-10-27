namespace Tests.Fakes;

public class FakeBusinessEntityRequestView
{
    public void SetAge(int age) => Age = age;
    
#pragma warning disable CS8618
    public string Value { get; set; }
#pragma warning restore CS8618
    public int Age { get; set; }
    public int FakeBusinessEntity2Id { get; set; }
}

public class FakeBusinessEntity2RequestView
{
#pragma warning disable CS8618
    public string Dni { get; set; }
#pragma warning restore CS8618
    public int FakeBusinessEntityId { get; set; }
}