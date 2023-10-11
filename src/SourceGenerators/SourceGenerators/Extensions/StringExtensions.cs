namespace Oluso.SourceGenerators.Extensions;

public static class StringExtensions
{
    public static string SimpleClassName(this string source)
    {
        return source.EndsWith("Controller")
            ? source.Substring(0, source.Length - 10)
            : source.EndsWith("Service")
                ? source.Substring(0, source.Length - 7)
                : source.EndsWith("Repository")
                    ? source.Substring(0, source.Length - 10)
                    : source;
    }    
}