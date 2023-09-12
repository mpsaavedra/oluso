using Oluso.Data.Specifications;

namespace Oluso.Data.Extensions;

/// <summary>
/// <see cref="CompilableSpecification{T}"/> extensions
/// </summary>
public static class CompilableSpecificationsExtensions
{
    /// <summary>
    /// <see cref="CompilableAndSpecification{T}"/> shortcut
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static CompilableSpecification<T> And<T>(this CompilableSpecification<T> left,
        CompilableSpecification<T> right) =>
        new CompilableAndSpecification<T>(left, right);

    /// <summary>
    /// <see cref="CompilableOrSpecification{T}"/> shortcut
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static CompilableSpecification<T> Or<T>(this CompilableSpecification<T> left,
        CompilableSpecification<T> right) =>
        new CompilableOrSpecification<T>(left, right);

    /// <summary>
    /// <see cref="CompilableNotSpecification{T}"/> shortcut
    /// </summary>
    /// <param name="specification"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static CompilableSpecification<T> Not<T>(this CompilableSpecification<T> specification) =>
        new CompilableNotSpecification<T>(specification);

    /// <summary>
    /// returns a <see cref="CompilableSpecification{T}"/> that combines with <b>And</b> the list
    /// of <see cref="CompilableSpecification{T}"/>
    /// </summary>
    /// <param name="target"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static CompilableSpecification<T> CombineWithAnds<T>(this IEnumerable<CompilableSpecification<T>> target)
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
    /// returns a <see cref="CompilableSpecification{T}"/> that combines with <b>Or</b> the list
    /// of <see cref="CompilableSpecification{T}"/>
    /// </summary>
    /// <param name="target"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static CompilableSpecification<T> CombineWithOrs<T>(this IEnumerable<CompilableSpecification<T>> target)
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