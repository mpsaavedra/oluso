using Oluso.Configuration.Abstractions.Extensions;

namespace Oluso.Configuration.Hosting.Providers.Filesystem;

/// <summary>
/// Filesystem options to configure <see cref="FilesystemProvider"/>
/// </summary>
public class FilesystemProviderOptions
{
    /// <summary>
    /// get <see cref="FilesystemProviderSettings"/>
    /// </summary>
    public FilesystemProviderSettings Settings { get; } = new();

    /// <summary>
    /// returns a new <see cref="FilesystemProviderOptions"/> instance
    /// </summary>
    public FilesystemProviderOptions()
    {
    }

    /// <summary>
    /// set path to settings container directory
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FilesystemProviderOptions WithPath(string path)
    {
        path.ToNullEmptyOrWhitespaceThrow($"Path could not be null, empty or whitespace");
        Settings.Path = path;
        return this;
    }
    
    /// <summary>
    /// set if include sub directories when searching for settings changes
    /// </summary>
    /// <param name="includeSubDirectories"></param>
    /// <returns></returns>
    public FilesystemProviderOptions WithIncludeSubDirectories(bool includeSubDirectories)
    {
        Settings.IncludeSubDirectories = includeSubDirectories;
        return this;
    }
    
    /// <summary>
    /// set username to access
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public FilesystemProviderOptions WithUserName(string username)
    {
        Settings.UserName = username;
        Settings.UserName = username;
        return this;
    }
    
    /// <summary>
    /// set access password
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public FilesystemProviderOptions WithPassword(string password)
    {
        Settings.Password = password;
        return this;
    }
    
    /// <summary>
    /// set domain
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public FilesystemProviderOptions WithDomain(string domain)
    {
        Settings.Domain = domain;
        return this;
    }
}