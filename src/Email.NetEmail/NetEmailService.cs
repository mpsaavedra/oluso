using System.Net;
using System.Net.Mail;
using Oluso.Email.Settings;

namespace Oluso.Email.NetEmail;


/// <summary>
/// <inheritdoc cref="EmailService"/>
/// </summary>
public class NetEmailService : EmailService, IEmailService
{
    /// <summary>
    /// returns a new <see cref="NetEmailService"/> instance
    /// </summary>
    /// <param name="settings"></param>
    public NetEmailService(IEmailSettings settings) : base(settings)
    {
        EmailSettings = settings;
    }

    /// <summary>
    /// <inheritdoc cref="IEmailService.SendEmailAsync"/>
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override async Task<bool> SendEmailAsync(Models.Email email)
    {
        if (EmailSettings.Enable == false)
        {
            return false;
        }
        
        SetupServer();

        var msg = new MailMessage();
        msg.IsBodyHtml = true;
        msg.From = new MailAddress(email.From.FromEmail, email.From.FromName);
        msg.Subject = email.Subject;
        msg.Body = email.Body?.Content;
        
        email.To.ForEach(to => msg.To.Add(new MailAddress(to.FromEmail, to.FromName)));
        email.Bcc?.ForEach(bcc => msg.Bcc.Add(new MailAddress(bcc.FromEmail, bcc.FromName)));
        email.Cc?.ForEach(cc => msg.CC.Add(new MailAddress(cc.FromEmail, cc.FromName)));
        email.Attachments?.ForEach(attachment =>
        {
            attachment.AttachmentPathDirectory.IsDirectoryExistsThrow();
            if (!attachment.IsAttachment) return;
            foreach (var file in attachment.AttachmentPathDirectory.ReadAllBytes())
            {
                msg.Attachments.Add(new Attachment(new MemoryStream(file.fileBytes), file.filename));
            }
        });
        email.Zip?.ForEach(zip =>
        {
            zip.ZipPathDirectory.IsDirectoryExistsThrow();
            if (!zip.IsCompressed) return;
            if (EmailSettings.AttachmentPathDirectory != null)
                Helpers.ZipFiles(EmailSettings.AttachmentPathDirectory, zip.ZipPathDirectory, zip.DeleteAfterAttach);
            var file = zip.ZipPathDirectory.ReadAllBytes().FirstOrDefault();
            msg.Attachments.Add(new Attachment(new MemoryStream(file.fileBytes), file.filename));
            if (zip.DeleteAfterAttach)
            {
                Helpers.DeleteFilesDirectory(zip.ZipPathDirectory, true);
            }
        });

        using (var client = new SmtpClient(EmailSettings.Host, EmailSettings.Port!.Value))
        {
            if (EmailSettings.UseCredentials != null && EmailSettings.UseCredentials.Value)
                client.Credentials = new NetworkCredential(EmailSettings.UserName, EmailSettings.Password);

            if (EmailSettings.UseSsl != null) client.EnableSsl = EmailSettings.UseSsl.Value;

            client.SendCompleted += delegate(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
            {
                if (e.Error != null)
                {
                    Helpers.Show(Messages.EmailCouldNotBeSend());
                }
                else
                {
                    Helpers.Show(Messages.EmailSentSuccessfully());
                }

                var userMessage = e.UserState as MailMessage;
                if (userMessage != null)
                {
                    userMessage.Dispose();
                }
            };
            Helpers.Show(Messages.EmailSendingPleaseWait());
            await client.SendMailAsync(msg);
        }
    
        return true;
    }
}