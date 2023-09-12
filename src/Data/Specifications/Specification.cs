using Oluso.Data.Extensions;
using Oluso.Extensions;

namespace Oluso.Data.Specifications;

/// <summary>
/// Base class for all specifications
/// </summary>
/// <remarks>
/// An specification is a function or expression that could be evaluated over an entity to get a binary result.
/// The predicate definition with a domain structure like an Specification allows the reusability and allows the
/// combination of the expressions to obtain more complex predicates
/// </remarks>>
/// <typeparam name="T">Entity type to apply the specification</typeparam>
public abstract record Specification<T>
{
    /// <summary>
    /// Transform current specification into a callable function
    /// </summary>
    public abstract Func<T, bool> ToFunc();
    
     /// <summary>
     /// Simplified And operator ( &amp; )
     /// </summary>
     /// <param name="left"></param>
     /// <param name="right"></param>
     /// <returns></returns>
    public static Specification<T> operator &(Specification<T> left, Specification<T> right) => left.And(right);

    /// <summary>
    /// simplified Or operator ( | )
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Specification<T> operator |(Specification<T> left, Specification<T> right) => left.Or(right);

    /// <summary>
    /// simplified Not operator ( ! )
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    public static Specification<T> operator !(Specification<T> specification) => specification.Not();

    /// <summary>
    /// Always return false when evaluate any <see cref="Specification{T}"/>
    /// </summary>
    public static readonly Specification<T> False = new FalseSpecification();
    
    /// <summary>
    /// Always return true when evaluate any <see cref="Specification{T}"/>
    /// </summary>
    public static readonly Specification<T> True = new TrueSpecification();

    private record FalseSpecification : Specification<T>
    {
        public override Func<T, bool> ToFunc() => _ => false;
    }

    private record TrueSpecification : Specification<T>
    {
        public override Func<T, bool> ToFunc() => _ => true;
    }
    
    /// <summary>
    /// Implicit conversion of the <see cref="Specification{T}"/> to <see cref="Func{TResult}"/>
    /// </summary>
    public static implicit operator Func<T, bool>(Specification<T> specification) => specification.ToFunc();
}