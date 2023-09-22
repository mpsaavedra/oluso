namespace Plugins.Sample.Shared;

/// <summary>
/// payload passed as parameter among the middleware plugins
/// </summary>
public class TestPayload
{
    public string Name { get; set; } = "Test payload";
    public int Counter { get; set; } = 1;
}