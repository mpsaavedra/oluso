using System.Runtime.Serialization;

namespace Oluso;

/// <summary>
/// Core messages. this class uses the resources file to retrieve messages in different languages
/// </summary>
public static class Messages
{
    /// <summary>
    /// returns a message if item is null or empty
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static string ItemNullException(string item) => string.Format(Resources.MsgItemNullException, item);

    /// <summary>
    /// returns a message when source is null or contains null members
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string NullOrEmpty(string source) => string.Format(Resources.MsgNullOrEmpty, source);

    /// <summary>
    /// returns a message of comparison could not be false
    /// </summary>
    /// <returns></returns>
    public static string ComparisonCouldNotBeFalse() => string.Format(Resources.MsgComparisonMustBeFalse);
    
    /// <summary>
    /// returns a message of comparison could not be true
    /// </summary>
    /// <returns></returns>
    public static string ComparisonCouldNotBeTrue() => string.Format(Resources.MsgComparisonMustBeTrue);

    /// <summary>
    /// returns a message Operation had fail
    /// </summary>
    /// <returns></returns>
    public static string HadFail() => string.Format(Resources.MsgHadFail);

    /// <summary>
    /// returns a bytesName or any of his members could be null
    /// </summary>
    /// <param name="bytesName"></param>
    /// <returns></returns>
    public static string NullOrAnyNull(string bytesName) => string.Format(Resources.MsgNullOrAnyNull, bytesName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string InvalidFormat(string source) => string.Format(Resources.MsgInvalidFormat, source);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string NotFound(string source) => string.Format(Resources.MsgNotFound, source);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static string Greater(string source, long max) => string.Format(Resources.MsgGreater, source, max);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    public static string Lower(string source, long min) => string.Format(Resources.MsgLower, source, min);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static string IsInRange(string source, long min, long max) => string.Format(Resources.MsgInRange, source, min, max);
}