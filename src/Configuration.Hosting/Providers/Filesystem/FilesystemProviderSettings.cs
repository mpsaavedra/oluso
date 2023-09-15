namespace Oluso.Configuration.Hosting.Providers.Filesystem;

/// <summary>
/// Settings to configure <see cref="FilesystemProvider"/>. this settings also include
/// remote host authentication parameters if needed
/// </summary>
public class FilesystemProviderSettings
{
    /// <summary>
    /// Path to the Configuration files
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// The search string to use to filter the configuration file names. Defaults
    /// to all files ('*')
    /// </summary>
    public string SearchPattern { get; set; }

    /// <summary>
    /// if true it will search for files also in subdirectories
    /// </summary>
    public bool IncludeSubDirectories { get; set; } = true;

    /// <summary>
    /// authentication username
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// authentication password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// authentication domain if any
    /// </summary>
    public string Domain { get; set; }
}