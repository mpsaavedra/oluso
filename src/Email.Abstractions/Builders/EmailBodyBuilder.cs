using Oluso.Email.Models;

namespace Oluso.Email.Builders;

/// <summary>
/// fluent api <see cref="EmailBody"/> builder
/// </summary>
public class EmailBodyBuilder
{
    private string _templatePath;
    private string _templateName;
    private Dictionary<string, string>? _data = new();
    
#pragma warning disable CS8618
    private EmailBodyBuilder()
#pragma warning restore CS8618
    {
    }

    /// <summary>
    /// returns a new <see cref="EmailBodyBuilder"/> instance
    /// </summary>
    public static EmailBodyBuilder New => new();

    /// <summary>
    /// set the template path where templates are located
    /// </summary>
    /// <param name="templatePathDirectory"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public EmailBodyBuilder WithTemplatePathDirectory(string templatePathDirectory)
    {
        if (!Directory.Exists(templatePathDirectory))
            throw new ApplicationException(Messages.EmailTemplateDirectoryNotFound(templatePathDirectory));

        _templatePath = templatePathDirectory;
        return this;
    }

    /// <summary>
    /// set the template name to use when generating html
    /// </summary>
    /// <param name="templateName"></param>
    /// <returns></returns>
    public EmailBodyBuilder WithTemplateName(string templateName)
    {
        _templateName = templateName;
        return this;
    }

    /// <summary>
    /// returns a new <see cref="EmailBody"/> loading template from provided information.
    /// </summary>
    /// <returns></returns>
    public EmailBody BuildFromTemplate() => EmailBody.CreateFromHtmlTemplate(_templateName, _templatePath, _data);

    /// <summary>
    /// returns a new <see cref="EmailBody"/> in plain text from provided string
    /// </summary>
    /// <param name="plainBody"></param>
    /// <returns></returns>
    public EmailBody BuildFromText(string plainBody) => EmailBody.CreateTextBody(plainBody);

    /// <summary>
    /// returns a new <see cref="EmailBody"/> in html format from provided string
    /// </summary>
    /// <param name="htmlBody"></param>
    /// <returns></returns>
    public EmailBody BuildFromHtml(string htmlBody) => EmailBody.CreateHtmlBody(htmlBody);
}