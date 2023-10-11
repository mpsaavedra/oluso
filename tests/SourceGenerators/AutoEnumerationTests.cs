using System.Numerics;
using Oluso.Attributes;

namespace Tests.SourceGenerators;

public class AutoEnumerationTests
{
    [Fact]
    public void ItemsList()
    {
    }
}

[AutoEnumeration]
public partial class Planet
{
    public static readonly Planet Mercury = new ("Mercury");
    public static readonly Planet Venus = new ("Venus");
    public static readonly Planet Earth = new ("Earth");
    public static readonly Planet Mars = new ("Mars");
    public static readonly Planet Jupiter = new ("Jupiter");
    public static readonly Planet Saturn = new ("Saturn");
    public static readonly Planet Uranus = new ("Uranus");
    public static readonly Planet Neptune = new ("Neptune");
    
    public string Name { get; }

    private Planet(string name)
    {
        Name = name;
    }
}