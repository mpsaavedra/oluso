using Oluso.Extensions;

namespace Oluso.Data.Specifications;

/// <summary>
/// Represents the specification which indicates the semantics opposite to the given specification.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public record NotSpecification<T>(Specification<T> Specification) : Specification<T>
{
    /// <summary>
    /// return the linq result of the specification
    /// </summary>
    /// <returns></returns>
    public override Func<T, bool> ToFunc() => Specification.ToFunc().Not();
}