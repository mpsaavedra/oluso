using Oluso.Email.Extensions;

namespace Oluso.Email.Models;

/// <summary>
/// represents and email body content
/// </summary>
public class EmailBody
{
    /// <summary>
    /// Email body content
    /// </summary>
    public string Content { get; }

    private EmailBody(string body) => Content = body;

    /// <summary>
    /// creates an email body loading a template and parsing the data Dictionary{string, string} into
    /// the marked template tags
    /// <code>
    ///  // template : email.html
    ///  <p>
    ///    Hello %%user%%
    ///  </p>
    ///  // simple usage will be
    ///  var data = new Dictionary{string, string}() { "user", "Michel" }
    ///  var body = EmailBody.CreateFromHtmlTemplate("email.html", data);
    ///  // body is now
    ///  // body.Content;// "Hello Michel" 
    /// </code> 
    /// </summary>
    /// <param name="templateName"></param>
    /// <param name="templatePathDirectory"></param>
    /// <param name="data"></param>
    /// <param name="templateTags">tags to substitute in the template comma separated for begin and end. default <b>%%,%%</b></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static EmailBody CreateFromHtmlTemplate(string templateName, string templatePathDirectory,
        Dictionary<string, string>? data = null, string templateTags = "%%,%%")
    {
        var filePath = Path.Combine(templatePathDirectory, templateName);
        
        if (!Directory.Exists(templatePathDirectory))
            throw new ApplicationException(Messages.EmailTemplateDirectoryNotFound(templatePathDirectory));
        
        if (!File.Exists(filePath))
            throw new ApplicationException(Messages.EmailTemplateNotFound(filePath));

        var template = new StreamReader(filePath).ReadToEnd();
        if (data?.Any() == true)
        {
            foreach (var item in data)
            {
                template = template.ReplaceAll($"{templateTags.Split(',')[0]}{item.Key}{templateTags.Split(',')[1]}", item.Value);
            }
        }
        return new EmailBody(template);
    }

    /// <summary>
    /// create an email body from html
    /// </summary>
    /// <param name="htmlBody"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static EmailBody CreateHtmlBody(string htmlBody)
    {
        if (string.IsNullOrEmpty(htmlBody) || string.IsNullOrWhiteSpace(htmlBody))
            throw new ApplicationException(Messages.EmailBodyCouldNotBeNull());
        return new EmailBody(htmlBody);
    }

    /// <summary>
    /// the email body from a plaint text
    /// </summary>
    /// <param name="plainTextBody"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static EmailBody CreateTextBody(string plainTextBody)
    {
        if (string.IsNullOrEmpty(plainTextBody) || string.IsNullOrWhiteSpace(plainTextBody))
            throw new ApplicationException(Messages.EmailBodyCouldNotBeNull());
        return new EmailBody($"{plainTextBody}");
    }
}