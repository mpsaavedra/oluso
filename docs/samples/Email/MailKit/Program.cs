using Microsoft.Extensions.DependencyInjection;
using Oluso.Email;
using Oluso.Email.Extensions;
using Oluso.Email.Models;

var services = new ServiceCollection();
services.AddEmailService(options =>
{
    options.WithMailKit(cfg => 
        cfg
            .WithPort(1025)
            .WithUseSsl(false));
});
var provider = services.BuildServiceProvider();
var svr = provider.GetRequiredService<IEmailService>();

await svr.SendEmailAsync(Email
    .builder
    .WithSubject("Plain text email")
    .WithFrom(EmailAddress.Builder.WithFromEmail($"sender@example.com").Build())
    .WithTo(EmailAddress.Builder.WithFromEmail("receiver@example.com").Build())
    .WithBody(EmailBody.CreateTextBody("This is a plain text email"))
    .Build());

await svr.SendEmailAsync(Email
    .builder
    .WithSubject("HTML email")
    .WithFrom(EmailAddress.Builder.WithFromEmail($"sender@example.com").Build())
    .WithTo(EmailAddress.Builder.WithFromEmail("receiver@example.com").Build())
    .WithBody(EmailBody.CreateHtmlBody("<p>This is an HTML  email</p>"))
    .Build());

await svr.SendEmailAsync(Email
    .builder
    .WithSubject("Template email")
    .WithFrom(EmailAddress.Builder.WithFromEmail($"sender@example.com").Build())
    .WithTo(EmailAddress.Builder.WithFromEmail("receiver@example.com").Build())
    .WithBody(EmailBody.CreateFromHtmlTemplate(
        "default.html",
        "templates",
        new Dictionary<string, string>()
        {
            {"SUBJECT", "Template email"},
            {"USERNAME", "Tester"},
        }))
    .Build());