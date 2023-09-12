using System.Linq.Expressions;
using Oluso.Data.Extensions;

namespace Oluso.Data.Specifications;

/// <summary>
/// Base class of the the <see cref="Specification{T}"/> that could be translated to
/// and SQL query by an ORM
/// </summary>
/// <typeparam name="T">Entity type to apply the specification</typeparam>
public abstract record CompilableSpecification<T> : Specification<T>
{
    /// <summary>
    /// Returns the current specification as an expression tree
    /// </summary>
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <inheritdoc />
    public override Func<T, bool> ToFunc() => ToExpression().Compile();

    /// <summary>
    /// simplified and operator ( &amp; )
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static CompilableSpecification<T> operator &(CompilableSpecification<T> left,
        CompilableSpecification<T> right) => left.And(right);

    /// <summary>
    /// simplified or operator ( | )
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static CompilableSpecification<T> operator |(CompilableSpecification<T> left,
        CompilableSpecification<T> right) => left.Or(right);

    /// <summary>
    /// simplified not operator ( ! )
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    public static CompilableSpecification<T> operator !(CompilableSpecification<T> specification) =>
        specification.Not();

    /// <summary>
    /// <see cref="CompilableSpecification{T}"/> that always returns false when applied to any entity
    /// </summary>
    public new static readonly CompilableSpecification<T> False = new CompilableFalseSpecification();

    /// <summary>
    /// <see cref="CompilableSpecification{T}"/> that always returns true when applied to any entity
    /// </summary>
    public new static readonly CompilableSpecification<T> True = new CompilableTrueSpecification();

    private record CompilableFalseSpecification : CompilableSpecification<T>
    {
        public override Expression<Func<T, bool>> ToExpression() => _ => false;
    }

    private record CompilableTrueSpecification : CompilableSpecification<T>
    {
        public override Expression<Func<T, bool>> ToExpression() => _ => true;
    }

    /// <summary>
    /// implicit cast of <see cref="CompilableSpecification{T}"/> to <see cref="Expression"/>
    /// </summary>
    public static implicit operator Expression<Func<T, bool>>(CompilableSpecification<T> specification) =>
        specification.ToExpression();
}