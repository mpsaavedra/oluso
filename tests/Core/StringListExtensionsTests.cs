using Oluso.Extensions;
using Shouldly;

namespace Tests.Core;

public class StringListExtensionsTests
{
    [Fact]
    public void StringListExtensionsTests_ToString()
    {
        var data = new List<string>()
        {
            "one", "two", "three", "four", "five"
        };
        data.ToPlainString().ShouldNotBeNull();
        data.ToPlainString().ShouldBe("one, two, three, four, five,");
    }
}