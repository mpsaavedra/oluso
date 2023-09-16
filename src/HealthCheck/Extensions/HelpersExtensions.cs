using System.Collections;

namespace Oluso.HealthCheck.Extensions;

/// <summary>
/// Helper extensions
/// </summary>
public static class HelperExtensions
{
    /// <summary>
    /// checks if values are null or empty returns true if any it is
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool NullOrEmpty(params object?[] values)
    {
        if (values == null || !values.Any()) return true;
        var fails = values.Where(value =>
        {
            if (value == null) return true;

            if (value.IsInstanceOf(typeof(string)))
                return string.IsNullOrEmpty((string)value);

            switch (value)
            {
                case string s when s.Length < 1:
                case Array a when a.Length < 1:
                case ICollection c when c.Count < 1:    
                case IEnumerable e when e.Cast<object>().Any():
                    return true;
                default:
                    return false;
            }
        });
        return fails.Any();
    }
    
    /// <summary>
    /// combine custom action values in action with default values of his type. It applies
    /// the override properties to the result value.
    /// <code>
    /// class Config
    /// {
    ///     public int Value { get; set; } = 1;
    ///     public int Value2 { get; set; } = 2;
    /// }
    ///
    /// public class ConfigConsumer
    /// {
    ///     public Config Configure(Action{Config} configuration)
    ///     {
    ///         var opts = configuration.ToHealthCheckConfigureOrDefault();
    ///         returns opts;
    ///     }
    /// }
    ///
    /// class ConfigTest
    /// {
    ///     public void Test()
    ///     {
    ///         var consumer = new ConfigConsumer();
    ///         var config = new Config { Value = 3 };
    ///         var result = consumer.Configure((opt) => opt = config);
    ///         // result.value1 == 1;
    ///         // result.Value2 == 3;
    ///     }
    /// }
    /// </code>
    /// </summary>
    /// <param name="action"></param>
    /// <param name="overrideDefaultOptions"></param>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public static TOptions ToHealthCheckConfigureOrDefault<TOptions>(this Action<TOptions?> action,
        TOptions? overrideDefaultOptions = null!)
        where TOptions : class, new()
    {
        overrideDefaultOptions ??= new TOptions();
        action.Invoke(overrideDefaultOptions);

        return overrideDefaultOptions;
    }
    
    /// <summary>
    /// check if source is null or empty if it is is Throws and <see cref="ApplicationException"/>, if not
    /// it return the source value
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsNullOrEmptyThrow<TSource>(this TSource source, string message) =>
        source.Throw(() => NullOrEmpty(source), message);
    
    private static TSource Throw<TSource>(this TSource source, Func<bool> func, string message)
    {
        if(source.Errors(func))
            throw new ApplicationException(message);
        return source;
    }

    /// <summary>
    /// check for possible errors in the source, errors could be:<br/>
    /// <li>source to be null</li>
    /// <li>func to be null</li>
    /// <li>result of the validation function return true</li>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="func">validation function to execute</param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static bool Errors<TSource>(this TSource source, Func<bool> func) =>
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        source == null || func == null || func();
    
    /// <summary>
    /// check if obj is instance of a given type
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="checkType"></param>
    /// <returns></returns>
    public static bool IsInstanceOf(this object obj, Type checkType) =>
        obj.GetType().IsOfType(checkType);
    
    /// <summary>
    /// checks if is of a given type 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="checkType"></param>
    /// <returns></returns>
    public static bool IsOfType(this Type source, Type checkType) =>
        source.IsInstanceOfType(checkType);
}