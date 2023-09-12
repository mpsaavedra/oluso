using Oluso.Extensions;

namespace Oluso.Data.Specifications;

/// <summary>
/// Represents the combined specification which indicates that both of the given
/// specifications should be satisfied by the given object.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public sealed record AndSpecification<T>(Specification<T> Left, Specification<T> Right) : Specification<T>
{
    /// <summary>
    /// return the linq result of the specification
    /// </summary>
    /// <returns></returns>
    public override Func<T, bool> ToFunc() => Left.ToFunc().And(Right.ToFunc());
}