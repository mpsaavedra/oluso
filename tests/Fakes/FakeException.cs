namespace Tests.Fakes;

public class FakeException : Exception
{
    public FakeException(string msg) : base(msg)
    {
    }

    public FakeException()
    {
    }
}