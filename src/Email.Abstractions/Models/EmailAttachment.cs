using Oluso.Email.Builders;

namespace Oluso.Email.Models;

/// <summary>
/// represents an Email attachment file
/// </summary>
public class EmailAttachment
{
    /// <summary>
    /// returns a new <see cref="EmailAttachment"/> instance
    /// </summary>
    /// <param name="attachmentPathDirectory"></param>
    /// <exception cref="ApplicationException"></exception>
    public EmailAttachment(string attachmentPathDirectory)
    {
        AttachmentPathDirectory = attachmentPathDirectory;
        if (!IsAttachment)
            throw new ApplicationException(Messages.EmailAttachmentNotFound(attachmentPathDirectory));
    }
    
    /// <summary>
    /// returns a new <see cref="EmailAttachmentBuilder"/> instance
    /// </summary>
    public static EmailAttachmentBuilder Builder => EmailAttachmentBuilder.New;
    
    /// <summary>
    /// the path to the attachment file
    /// </summary>
    public string AttachmentPathDirectory { get; private set; }

    /// <summary>
    /// true if the attachment is set and exists
    /// </summary>
    public bool IsAttachment => 
        string.IsNullOrEmpty(AttachmentPathDirectory) &&
        File.Exists(AttachmentPathDirectory);
}