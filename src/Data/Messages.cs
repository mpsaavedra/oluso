namespace Oluso.Data;

/// <summary>
/// Messages
/// </summary>
public static class Messages
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <returns></returns>
    public static string EntityWithIdNotFound<TKey>(TKey id) => string.Format(Resources.MsgEntityWithIdNotFound, id);
}