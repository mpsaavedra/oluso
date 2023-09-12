using System.Linq.Expressions;
using Oluso.Data.Extensions;
using Oluso.Extensions;

namespace Oluso.Data.Specifications;

/// <summary>
/// <see cref="CompilableSpecification{T}"/> that applies the Not operator to compare
/// provided members
/// </summary>
/// <typeparam name="T"></typeparam>
public record CompilableNotSpecification<T>(CompilableSpecification<T> Specification) : CompilableSpecification<T>
{
    /// <summary>
    /// Gets the LINQ expression which represents the current specification.
    /// </summary>
    /// <returns>The LINQ expression.</returns>
    public override Expression<Func<T, bool>> ToExpression() => Specification.ToExpression().Not();
}