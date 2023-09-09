namespace Oluso;

/// <summary>
/// Core messages. this class uses the resources file to retrieve messages in different languages
/// </summary>
public static class Messages
{
    /// <summary>
    /// retrieve a message if item is null or empty
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static string ItemNullException(string item) => string.Format(Resources.MsgItemNullException, item);
}