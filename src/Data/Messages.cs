namespace Oluso.Data;

#pragma warning disable CS1591
/// <summary>
/// Messages
/// </summary>
public static class Messages
{
    public static string EntityWithIdNotFound<TKey>(TKey id) => string.Format(Resources.MsgEntityWithIdNotFound, id);
    
    public static string MsgGreaterThan(string source, int min) =>
        string.Format(Resources.MsgGreaterThan, source, min);
}
#pragma warning restore CS1591