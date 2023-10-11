using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oluso.SourceGenerators.Extensions;

namespace Oluso.SourceGenerators.Generators;

[Generator]
public class AutoEnumerationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var types = context
            .SyntaxProvider
            .CreateSyntaxProvider(IsValid, GetRequestedType)
            .Where(x => x is not null)
            .Collect();
        context.RegisterSourceOutput(types, GenerateCode);
    }

    private bool IsValid(SyntaxNode node, CancellationToken cancellationToken)
    {
        if (node is not AttributeSyntax attribute)
            return false;
        var name = ExtractName(attribute.Name);

        return name is "AutoEnumeration" or 
            "AutoEnumerationAttribute" or 
            nameof(Oluso.Attributes.AutoEnumerationAttribute);
    }

    private static void GenerateCode(SourceProductionContext context, 
        ImmutableArray<ITypeSymbol?> types)
    {
        if (types.IsDefaultOrEmpty) return;

        foreach (var type in types)
        {
            if (type is null) continue;
            var code = BuildCode(type);
            var (ns, name) = type.GetNameAndNamespace();
            context.AddSource($"{ns}{name}.g.cs", code);
            
        }
    }

    public static string BuildCode(ITypeSymbol type)
    {
        var ns = type.ContainingNamespace.IsGlobalNamespace
            ? null
            : type.ContainingNamespace.ToString();
        var name = type.Name;
        var items = GetItemNames(type);
        return $@"{Constants.CodeHeader}
using System.Collections.Generic;

namespace {(ns is null ? null : $@"")};

partial class {name}
{{
    private IReadOnlyList<{name}> _items;
    public static IReadOnlyList<{name}> Items => _items ??= GetItems();

    private static IReadOnlyList<{name}> GetItems()
    {{
     return new[] {{ {String.Join("", "", items)} }};
    }}
}}";
    }

    private static string? ExtractName(NameSyntax? name)
    {
        return name switch
        {
            SimpleNameSyntax ins => ins.Identifier.Text,
            QualifiedNameSyntax qns => qns.Right.Identifier.Text,
            _ => null
        };
    }
    
    private static ITypeSymbol? GetRequestedType(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var attributeSyntax = (AttributeSyntax)context.Node;
        if (attributeSyntax.Parent?.Parent is not ClassDeclarationSyntax classDeclarationSyntax)
            return null;
        var type = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) as ITypeSymbol;
        return type is null || !IsEnumeration(type) ? null : type;
    }
    
    private static bool IsEnumeration(ISymbol type) =>
        type.GetAttributes()
            .Any(a =>
                a.AttributeClass?.Name == "AutoEnumerationAttribute" &&
                a.AttributeClass.ContainingNamespace is
                {
                    Name: "Oluso",
                    ContainingNamespace.IsGlobalNamespace: true
                });

    private static IEnumerable<string?> GetItemNames(ITypeSymbol type) =>
        type.GetMembers()
            .Select(m =>
            {
                if (!m.IsStatic ||
                    m.DeclaredAccessibility != Accessibility.Public ||
                    m is not IFieldSymbol field)
                    return null;

                return SymbolEqualityComparer.Default.Equals(field.Type, type)
                    ? field.Name
                    : null;
            })
            .Where(field => field is not null);
}