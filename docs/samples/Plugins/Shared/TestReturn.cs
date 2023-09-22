namespace Plugins.Sample.Shared;

public class TestReturn : TestPayload
{
    public override string ToString() => $"TestReturn(Name: {Name}, Count: {Counter})";
}