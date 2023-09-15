namespace Oluso.Email;
#pragma warning disable CS1591

/// <summary>
/// Messages used in the system
/// </summary>
public static class Messages
{
    public static string EmailAddressCouldNotBeNull() => string.Format(Resources.MsgEmailAddressCouldNotBeNull);

    public static string EmailFormatIsInvalid(string email) => string.Format(Resources.MsgEmailAddressIsInvalid, email);

    public static string? EmailBodyCouldNotBeNull() => Resources.MsgEmailBodyCoudNotBeNull;

    public static string EmailTemplateNotFound(string filePath) =>
        string.Format(Resources.MsgEmailTemplateNotFound, filePath);

    public static string EmailAttachmentCouldNotBeNull() => Resources.MsgEmailAttachmentCouldNotBeNull;

    public static string EmailAttachmentNotFound(string attachmentPathDirectory) =>
        string.Format(Resources.MsgEmailAttachmentNotFound, attachmentPathDirectory);

    public static string EmailTemplateDirectoryNotFound(string templateDir) =>
        String.Format(Resources.MsgEmailTemplateDirectoryNotFound, templateDir);

    public static string EmailZipDirectoryNotFound(string filePath) =>
        string.Format(Resources.MsgEmailZipDirectoryNotFound, filePath);

    public static string DirectoryNotFound(string directoryPath) =>
        string.Format(Resources.MsgDirectoryNotFound, directoryPath);

    public static string FileNotFound(string filePath) =>
        string.Format(Resources.MsgFileNotFound, filePath);

    public static string RequiredParameter(string paramName) =>
        string.Format(Resources.MsgRequiredParameter, paramName);

    public static string MustBeOfType(Type source, Type type) =>
        string.Format(Resources.MsgMustBeOfType, source, type);

    public static string EmailFromCouldNotBeNull() => Resources.MsgEmailFromCouldNotBeNull;

    public static string EmailToCouldNotBeNullOrEmpty() => Resources.MsgEmailToCouldNotBeNullOrEmpty;

    public static string EmailZipDirectoryCouldNotBeNull() => Resources.MsgEmailZipDirectoryCouldNotBeNull;

    public static string EmailCouldNotBeSend()
    {
        throw new NotImplementedException();
    }

    public static string EmailSentSuccessfully()
    {
        throw new NotImplementedException();
    }

    public static string EmailSendingPleaseWait()
    {
        throw new NotImplementedException();
    }

    public static string NullOrEmpty(string source) =>
        string.Format(Resources.MsgNullOrEmpty, source);
}

#pragma warning restore CS1591