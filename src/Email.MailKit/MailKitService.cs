using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Oluso.Email.Settings;

namespace Oluso.Email.MailKit;

/// <summary>
/// MailKit email sender
/// </summary>
public class MailKitService : EmailService, IEmailService
{
    /// <summary>
    /// returns a new <see cref="MailKitService"/> instance
    /// </summary>
    /// <param name="settings"></param>
    public MailKitService(IEmailSettings settings) : base(settings)
    {
    }

    /// <inheritdoc cref="IEmailService.SendEmailAsync"/>
    public override async Task<bool> SendEmailAsync(Models.Email email)
    {
        if (EmailSettings.Enable == false)
        {
            return false;
        }
        
        SetupServer();

        var msg = new MimeMessage();
        msg.From.Add(MailboxAddress.Parse($"{email.From.FromName} {email.From.FromEmail}"));
        msg.Subject = email.Subject;
        msg.Body = new TextPart(TextFormat.Html)
        {
            Text = email.Body?.Content
        };
        
        email.To?.ForEach(to => msg.To.Add(MailboxAddress.Parse($"{to.FromName} {to.FromEmail}")));
        email.Bcc?.ForEach(to => msg.To.Add(MailboxAddress.Parse($"{to.FromName} {to.FromEmail}")));
        email.Cc?.ForEach(to => msg.To.Add(MailboxAddress.Parse($"{to.FromName} {to.FromEmail}")));
        email.Attachments?.ForEach(attachment =>
        {
            
        });
        email.Zip?.ForEach(attachment =>
        {
            
        });

        using var client = new SmtpClient();
        await client.ConnectAsync(
            EmailSettings.Host, 
            EmailSettings.Port!.Value, 
            EmailSettings.UseSsl!.Value ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
        if(EmailSettings.UseCredentials == true)
        {
            await client.AuthenticateAsync(EmailSettings.UserName, EmailSettings.Password);
        }

        try
        {
            Messages.EmailSendingPleaseWait().Show();
            await client.SendAsync(msg);
            client.MessageSent += delegate(object? sender, MessageSentEventArgs args)
            {
                Messages.EmailSentSuccessfully().Show();
            };
        }
        catch (Exception e)
        {
            Messages.EmailCouldNotBeSend().Show();
        }
        await client.DisconnectAsync(true);

        return true;
    }
}