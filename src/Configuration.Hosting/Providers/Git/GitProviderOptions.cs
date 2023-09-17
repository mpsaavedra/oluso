using Oluso.Configuration.Abstractions.Extensions;

namespace Oluso.Configuration.Hosting.Providers.Git;

/// <summary>
/// Options for <see cref="GitProvider"/>
/// </summary>
public class GitProviderOptions
{
    /// <summary>
    /// <see cref="GitProviderSettings"/>
    /// </summary>
    public GitProviderSettings Settings { get; set; } = new();

    /// <summary>
    /// set the repository
    /// </summary>
    /// <param name="repositoryUrl"></param>
    /// <returns></returns>
    public GitProviderOptions WithRepositoryUrl(string repositoryUrl)
    {
        repositoryUrl.ToNullEmptyOrWhitespaceThrow(nameof(repositoryUrl));
        Settings.RepositoryUrl = repositoryUrl;
        return this;
    }
    
    /// <summary>
    /// set the username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public GitProviderOptions WithUsername(string username)
    {
        Settings.Username = username;
        return this;
    }
    
    /// <summary>
    /// set the password
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public GitProviderOptions WithPassword(string password)
    {
        Settings.Password = password;
        return this;
    }
    
    /// <summary>
    /// set the branch
    /// </summary>
    /// <param name="branch"></param>
    /// <returns></returns>
    public GitProviderOptions WithBranch(string branch)
    {
        branch.ToNullEmptyOrWhitespaceThrow(nameof(branch));
        Settings.Branch = branch;
        return this;
    }
    
    /// <summary>
    /// set local path
    /// </summary>
    /// <param name="localPath"></param>
    /// <returns></returns>
    public GitProviderOptions WithLocalPath(string localPath)
    {
        localPath.ToNullEmptyOrWhitespaceThrow(nameof(localPath));
        Settings.LocalPath = localPath;
        return this;
    }
    
    /// <summary>
    /// set search pattern
    /// </summary>
    /// <param name="searchPattern"></param>
    /// <returns></returns>
    public GitProviderOptions WithSearchPattern(string searchPattern)
    {
        searchPattern.ToNullEmptyOrWhitespaceThrow(nameof(searchPattern));
        Settings.SearchPattern = searchPattern;
        return this;
    }
    
    /// <summary>
    /// set the polling interval
    /// </summary>
    /// <param name="pollingInterval"></param>
    /// <returns></returns>
    public GitProviderOptions WithPollingInterval( TimeSpan pollingInterval)
    {
        Settings.PollingInterval = pollingInterval;
        return this;
    }
}