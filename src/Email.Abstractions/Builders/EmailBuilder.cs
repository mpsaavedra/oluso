using Oluso.Email.Models;

namespace Oluso.Email.Builders;

/// <summary>
/// fluent <see cref="EmailBuilder"/>
/// </summary>
public class EmailBuilder
{
    private EmailAddress _from;
    private List<EmailAddress> _to = new();
    private List<EmailAddress> _bcc = new();
    private List<EmailAddress> _cc = new();
    private string? _subject = null;
    private EmailBody? _body = null;
    private List<EmailAttachment> _attachments = new();
    private int _timeout = 100;
    private List<EmailZip> _zips = new();
    
#pragma warning disable CS8618
    private EmailBuilder()
#pragma warning restore CS8618
    {
    }

    /// <summary>
    /// returns a new <see cref="EmailBuilder"/> instance
    /// </summary>
    public static EmailBuilder New => new EmailBuilder();
    

    /// <summary>
    /// set the <see cref="EmailAddress"/> of the email sender
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    public EmailBuilder WithFrom(EmailAddress from)
    {
        _from = from;
        return this;
    }


    /// <summary>
    /// adds an address to the list of <see cref="EmailAddress"/> in the To email property
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    public EmailBuilder WithTo(EmailAddress to)
    {
        if (!_to.Any(x => x.FromEmail == to.FromEmail && x.FromName == to.FromName))
            _to.Add(to);
        return this;
    }


    /// <summary>
    /// set the list of <see cref="EmailAddress"/> in the To email property
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    public EmailBuilder WithTo(params EmailAddress[] to)
    {
        foreach (var t in to)
        {
            WithTo(t);
        }
        return this;
    }


    /// <summary>
    ///adds a new email to the list of <see cref="EmailAddress"/> in the Bcc email property
    /// </summary>
    /// <param name="bcc"></param>
    /// <returns></returns>
    public EmailBuilder WithBcc(EmailAddress bcc)
    {
        if (!_bcc.Any(x => x.FromEmail == bcc.FromEmail && x.FromName == bcc.FromName))
            _bcc.Add(bcc);
        return this;
    }

    /// <summary>
    /// the list of <see cref="EmailAddress"/> in the Bcc email property
    /// </summary>
    /// <param name="bcc"></param>
    /// <returns></returns>
    public EmailBuilder WithBcc(params EmailAddress[] bcc)
    {
        foreach (var b in bcc)
        {
            WithBcc(b);
        }
        return this;
    }


    /// <summary>
    /// adds a new address to the list of <see cref="EmailAddress"/> in the Cc email property
    /// </summary>
    /// <param name="cc"></param>
    /// <returns></returns>
    public EmailBuilder WithCc(EmailAddress cc)
    {
        if (!_cc.Any(x => x.FromEmail == cc.FromEmail && x.FromName == cc.FromName))
            _cc.Add(cc);
        return this;
    }

    /// <summary>
    /// set the list of <see cref="EmailAddress"/> in the Cc email property
    /// </summary>
    /// <param name="cc"></param>
    /// <returns></returns>
    public EmailBuilder WithCc(params EmailAddress[] cc)
    {
        foreach (var c in cc)
        {
            WithCc(c);
        }
        return this;
    }
    
    /// <summary>
    /// set the email subject
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public EmailBuilder WithSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    /// <summary>
    /// set the <see cref="EmailBody"/> content
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public EmailBuilder WithBody(EmailBody body)
    {
        _body = body;
        return this;
    }

    
    /// <summary>
    /// set the attachments list
    /// </summary>
    /// <param name="attachments"></param>
    /// <returns></returns>
    public EmailBuilder WithAttachments(List<EmailAttachment> attachments)
    {
        _attachments = attachments;
        return this;
    }
    
    /// <summary>
    /// adds an <see cref="EmailAttachment"/> to the attachments list
    /// </summary>
    /// <param name="attachment"></param>
    /// <returns></returns>
    public EmailBuilder WithAttachment(EmailAttachment attachment)
    {
        if(!_attachments.Any(x => x.AttachmentPathDirectory == attachment.AttachmentPathDirectory &&
                                 x.IsAttachment == attachment.IsAttachment))
            _attachments.Add(attachment);
        return this;
    }

    /// <summary>
    /// set the connection timeout
    /// </summary>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public EmailBuilder WithTimeout(int timeout)
    {
        _timeout = timeout;
        return this;
    }

    /// <summary>
    /// set the zips list
    /// </summary>
    /// <param name="zips"></param>
    /// <returns></returns>
    public EmailBuilder WithZip(List<EmailZip> zips)
    {
        _zips = zips;
        return this;
    }
    
    /// <summary>
    /// add a new <see cref="EmailZip"/> to the zips list
    /// </summary>
    /// <param name="zip"></param>
    /// <returns></returns>
    public EmailBuilder WithZip(EmailZip zip)
    {
        if (_zips.Any(x => x.ZipPathDirectory == zip.ZipPathDirectory))
            _zips.Add(zip);
        return this;
    }

    /// <summary>
    /// returns a new <see cref="Email"/> instance from provided information
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public Models.Email Build()
    {
        if (_from == null) throw new ApplicationException(Messages.EmailFromCouldNotBeNull());
        if (_to == null || !_to.Any()) throw new ApplicationException(Messages.EmailToCouldNotBeNullOrEmpty());
        
        return new(_from, _to, _subject, _body, _attachments, _bcc, _cc);
    }
}