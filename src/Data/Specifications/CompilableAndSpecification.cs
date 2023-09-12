using System.Linq.Expressions;
using Oluso.Data.Extensions;
using Oluso.Extensions;

namespace Oluso.Data.Specifications;

/// <summary>
/// <see cref="CompilableSpecification{T}"/> that applies the And operator to compare
/// provided members
/// </summary>
/// <param name="Left"></param>
/// <param name="Right"></param>
/// <typeparam name="T"></typeparam>
public record CompilableAndSpecification<T>
    (CompilableSpecification<T> Left, CompilableSpecification<T> Right) : CompilableSpecification<T>
{
    /// <summary>
    /// Gets the LINQ expression which represents the current specification.
    /// </summary>
    /// <returns>The LINQ expression.</returns>
    public override Expression<Func<T, bool>> ToExpression()
    {
        return Left.ToExpression().And(Right.ToExpression());
    }
}