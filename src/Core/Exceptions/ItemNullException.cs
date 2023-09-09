using System;

namespace Oluso.Exceptions;

/// <summary>
/// throw exception with a message that item must not be null
/// </summary>
public class ItemNullException : Exception
{
    /// <summary>
    /// returns a new <see cref="ItemNullException"/>
    /// </summary>
    /// <param name="itemName"></param>
    public ItemNullException(string itemName) : base(Messages.ItemNullException(itemName))
    {
    }

    /// <summary>
    /// returns a new <see cref="ItemNullException"/>
    /// </summary>
    public ItemNullException()
    {
    }
}