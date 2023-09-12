using Oluso.Data.Specifications;

namespace Oluso.Data.Extensions;

/// <summary>
/// <see cref="Specification{T}"/> extensions 
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Extends the <see cref="Specification{T}"/> with and <b>And</b> function that's
    /// a shortcut to the <see cref="AndSpecification{T}"/>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Specification<T> And<T>(this Specification<T> left, Specification<T> right) =>
        new AndSpecification<T>(left, right);

    /// <summary>
    /// Extends the <see cref="Specification{T}"/> with and <b>Or</b> function that's
    /// a shortcut to the <see cref="OrSpecification{T}"/>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Specification<T> Or<T>(this Specification<T> left, Specification<T> right) =>
        new OrSpecification<T>(left, right);

    /// <summary>
    /// Extends the <see cref="Specification{T}"/> with and <b>Not</b> function that's
    /// a shortcut to the <see cref="NotSpecification{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Specification<T> Not<T>(this Specification<T> specification) =>
        new NotSpecification<T>(specification);

    /// <summary>
    /// Combines the provided <see cref="Specification{T}"/> using And binary operator
    /// </summary>
    /// <param name="target"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Specification<T> CombineWithAnds<T>(this IEnumerable<Specification<T>> target)
    {
        var specifications = target.ToArray();
        switch (specifications.Length)
        {
            case 0:
            case 1:
                return specifications.First();
            default:
            {
                var result = specifications[0];
                for (var i = 1; i < specifications.Length; i++)
                {
                    var currentSpecification = specifications[i];
                    result = result.And(currentSpecification);
                }

                return result;
            }
        }
    }

    /// <summary>
    /// Combines the provided <see cref="Specification{T}"/> using Or binary operator
    /// </summary>
    /// <param name="target"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Specification<T> CombineWithOrs<T>(this IEnumerable<Specification<T>> target)
    {
        var specifications = target.ToArray();
        switch (specifications.Length)
        {
            case 0:
            case 1:
                return specifications.First();
            default:
            {
                var result = specifications[0];
                for (var i = 1; i < specifications.Length; i++)
                {
                    var currentSpecification = specifications[i];
                    result = result.Or(currentSpecification);
                }

                return result;
            }
        }
    }
}