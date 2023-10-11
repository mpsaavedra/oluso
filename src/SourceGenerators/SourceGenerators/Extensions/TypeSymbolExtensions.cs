using Microsoft.CodeAnalysis;

namespace Oluso.SourceGenerators.Extensions;

public static class TypeSymbolExtensions
{
    public static string? GetNamespace(this ITypeSymbol type) =>
        type.ContainingNamespace.IsGlobalNamespace
            ? null
            : type.ContainingNamespace.ToString();

    public static (string? ns, string name) GetNameAndNamespace(this ITypeSymbol type, bool plain = true) =>
        (type.GetNamespace(), plain ? type.Name : type.Name.SimpleClassName());
}