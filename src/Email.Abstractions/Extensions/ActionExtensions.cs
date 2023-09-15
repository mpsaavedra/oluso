namespace Oluso.Email.Extensions;

/// <summary>
/// Action related extensions
/// </summary>
public static class ActionExtensions
{
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
    ///         var opts = configuration.ToEmailConfigureOrDefault();
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
    public static TOptions ToEmailConfigureOrDefault<TOptions>(this Action<TOptions?> action,
        TOptions? overrideDefaultOptions = null!)
        where TOptions : class, new()
    {
        overrideDefaultOptions ??= new TOptions();
        action.Invoke(overrideDefaultOptions);

        return overrideDefaultOptions;
    }
}