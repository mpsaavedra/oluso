#nullable enable
using Oluso.Email;
using Oluso.Email.Extensions;
using Oluso.Email.Models;

// ReSharper disable once CheckNamespace
namespace Oluso.Email.Builders;

/// <summary>
/// builds a new Email address using fluent design
/// </summary>
public sealed class EmailAddressBuilder
{
    private string? _fromEmail = null;
    private string? _fromName = null;

    private EmailAddressBuilder()
    {
    }

    /// <summary>
    /// returns a new <see cref="EmailAddressBuilder"/> instance
    /// </summary>
    public static EmailAddressBuilder New => new();
    
    /// <summary>
    /// set the email from email address property
    /// </summary>
    /// <param name="fromEmail"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public EmailAddressBuilder WithFromEmail(string fromEmail)
    {
        if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrWhiteSpace(fromEmail))
            throw new ApplicationException(Messages.EmailAddressCouldNotBeNull());
        if (!fromEmail.IsEmail())
            throw new ApplicationException(Messages.EmailFormatIsInvalid(fromEmail));
        return this;
    }

    /// <summary>
    /// set the email from name property
    /// </summary>
    /// <param name="fromName"></param>
    /// <returns></returns>
    public EmailAddressBuilder WithFromName(string fromName)
    {
        if (string.IsNullOrEmpty(fromName) || string.IsNullOrWhiteSpace(fromName))
            throw new ApplicationException(Messages.EmailAddressCouldNotBeNull());
        _fromName = fromName;
        return this;
    }

    /// <summary>
    /// returns a valid email address from provided data
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public EmailAddress Build()
    {
        if (_fromEmail == null)
            throw new ApplicationException();
        return new EmailAddress(_fromEmail, _fromName);
    }
}