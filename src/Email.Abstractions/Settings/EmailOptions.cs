namespace Oluso.Email.Settings;

/// <summary>
/// Configuration options to set the email settings
/// </summary>
public class EmailOptions
{
    /// <summary>
    /// get the <see cref="EmailSettings"/> object with configuration to use
    /// </summary>
    public EmailSettings EmailSettings { get; private set; } = new();

    /// <summary>
    /// set if use credentials to authenticate to server or not
    /// </summary>
    /// <param name="useCredentials"></param>
    /// <returns></returns>
    public EmailOptions WithUseCredentials(bool useCredentials = Settings.EmailSettings.DefaultUseCredentials)
    {
        EmailSettings.UseCredentials = useCredentials;
        return this;
    }
    
    /// <summary>
    /// set if service is enable or not
    /// </summary>
    /// <param name="enabled"></param>
    /// <returns></returns>
    public EmailOptions WithEnabled(bool enabled = true)
    {
        EmailSettings.Enable = enabled;
        return this;
    }

    /// <summary>
    /// set server host name
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public EmailOptions WithHost(string host = Settings.EmailSettings.DefaultHost)
    {
        EmailSettings.Host = host;
        return this;
    }

    /// <summary>
    /// set server port
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public EmailOptions WithPort(int port = Settings.EmailSettings.DefaultPort)
    {
        EmailSettings.Port = port;
        return this;
    }
    
    /// <summary>
    /// set username
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public EmailOptions WithUserName(string userName=" ")
    {
        EmailSettings.UserName = userName;
        return this;
    }
    
    /// <summary>
    /// set user password
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public EmailOptions WithPassword(string password="")
    {
        EmailSettings.Password = password;
        return this;
    }
    
    /// <summary>
    /// set if use ssl or not in server connection
    /// </summary>
    /// <param name="useSsl"></param>
    /// <returns></returns>
    public EmailOptions WithUseSsl(bool useSsl=EmailSettings.DefaultUseSsl)
    {
        EmailSettings.UseSsl = useSsl;
        return this;
    }

    /// <summary>
    /// set email provider
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public EmailOptions WithProvider(string provider)
    {
        EmailSettings.Provider = provider;
        return this;
    }
}