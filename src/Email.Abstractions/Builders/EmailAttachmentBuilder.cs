using Oluso.Email.Models;

namespace Oluso.Email.Builders;

/// <summary>
/// Define a fluent api builder to build a valid <see cref="EmailAttachment"/> instance
/// </summary>
public class EmailAttachmentBuilder
{
    private string? _attachmentPathDirectory = null;
    
    /// <summary>
    /// returns a new <see cref="EmailAttachmentBuilder"/> instance
    /// </summary>
    public static EmailAttachmentBuilder New => new();

    /// <summary>
    /// set the attachment path directory
    /// </summary>
    /// <param name="attachmentPathDirectory"></param>
    /// <returns></returns>
    public EmailAttachmentBuilder WithAttachmentPathDirectory(string attachmentPathDirectory)
    {
        Validate(attachmentPathDirectory);
        _attachmentPathDirectory = attachmentPathDirectory;
        return this;
    }

    /// <summary>
    /// returns a new <see cref="EmailAttachment"/> instance from provided parameters
    /// </summary>
    /// <returns></returns>
    public EmailAttachment Builder()
    {
        Validate(_attachmentPathDirectory);
        return new(_attachmentPathDirectory!);
    }

    private void Validate(string? attachmentPathDirectory)
    {
        if (string.IsNullOrEmpty(attachmentPathDirectory) || string.IsNullOrWhiteSpace(attachmentPathDirectory))
            throw new ApplicationException(Messages.EmailAttachmentCouldNotBeNull());
    }
}