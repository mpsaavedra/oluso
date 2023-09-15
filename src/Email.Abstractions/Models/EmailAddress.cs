
using Oluso.Email.Builders;

// ReSharper disable once CheckNamespace
namespace Oluso.Email.Models;

/// <summary>
/// represent an email address
/// </summary>
public class EmailAddress
{
    private readonly string? _fromName;
    private readonly string _fromEmail;
    
    /// <summary>
    /// returns a new <see cref="EmailAddress"/> instance
    /// </summary>
    /// <param name="fromEmail"></param>
    /// <param name="fromName"></param>
    public EmailAddress(string fromEmail, string? fromName = null)
    {
        _fromEmail = fromEmail;
        _fromName = fromName;
    }
    
    /// <summary>
    /// returns an <see cref="EmailAddressBuilder"/> instance that uses a fluent api to
    /// create a new <see cref="EmailAddress"/>
    /// </summary>
    public static EmailAddressBuilder Builder => EmailAddressBuilder.New;

    /// <summary>
    /// fet the email address
    /// </summary>
    public string FromEmail => _fromEmail;

    /// <summary>
    /// get the name of email origin if is null uses the email address
    /// </summary>
    public string FromName => _fromName ?? _fromEmail;
}