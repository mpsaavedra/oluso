using Oluso.Email.Builders;

namespace Oluso.Email.Models;

/// <summary>
/// represent all required data of an email
/// </summary>
public class Email
{
    /// <summary>
    /// returns a new <see cref="Email"/> instance
    /// </summary>
    /// <param name="fromAddress"></param>
    /// <param name="toAddress"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <param name="attachments"></param>
    /// <param name="bcc"></param>
    /// <param name="cc"></param>
    public Email(EmailAddress fromAddress, List<EmailAddress> toAddress,
        string? subject = null, EmailBody? body = null, List<EmailAttachment>? attachments = null,
        List<EmailAddress>? bcc = null, List<EmailAddress>? cc = null)
    {
        From = fromAddress;
        To = toAddress ?? new();
        Bcc = bcc ?? new();
        Cc = cc ?? new();
        Subject = subject;
        Body = body;
        Attachments = attachments;
        Zip = new ();
    }
    
    /// <summary>
    /// returns a new <see cref="EmailBuilder"/> instance
    /// </summary>
    public static EmailBuilder builder => EmailBuilder.New;
    
    /// <summary>
    /// get email sender address
    /// </summary>
    public EmailAddress From { get; }
    
    /// <summary>
    /// get list of email To addresses
    /// </summary>
    public List<EmailAddress> To { get; }
    
    /// <summary>
    /// get list of email Dc addresses
    /// </summary>
    public List<EmailAddress>? Bcc { get; }
    
    /// <summary>
    /// get list with email Cc addresses
    /// </summary>
    public List<EmailAddress>? Cc { get; }
    
    /// <summary>
    /// get email subject
    /// </summary>
    public string? Subject { get; }
    
    /// <summary>
    /// get email body content
    /// </summary>
    public EmailBody? Body { get; }
    
    /// <summary>
    /// get list of attachment
    /// </summary>
    public List<EmailAttachment>? Attachments { get; }
    
    /// <summary>
    /// connection timeout
    /// </summary>
    public int Timeout { get; set; }
    
    /// <summary>
    /// get list of <see cref="EmailZip"/>
    /// </summary>
    public List<EmailZip> Zip { get; set; }
}