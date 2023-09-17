using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Oluso.Configuration.Abstractions;
using Oluso.Configuration.Abstractions.Helpers;

namespace Oluso.Configuration.Hosting.Providers.Filesystem;

/// <inheritdoc cref="IProvider"/>
public class FilesystemProvider : IProvider
{
    private readonly ILogger<FilesystemProvider> _logger;
    private readonly FilesystemProviderSettings _settings;
    private FileSystemWatcher _watcher;
    private Func<IEnumerable<string>, Task> _onChange;
    
    /// <inheritdoc cref="IProvider.Name"/>
    public string Name => $"Filesystem provider";
    
#pragma warning disable CS8618 
    /// <summary></summary>
    public FilesystemProvider(ILogger<FilesystemProvider> logger, FilesystemProviderSettings settings)
    {
#pragma warning restore CS8618
        _logger = logger;
        _settings = settings;

        if (string.IsNullOrWhiteSpace(settings.Path))
        {
            throw new ArgumentNullException(nameof(_settings.Path),
                $"{nameof(_settings.Path)} cannot be empty, null or whitespace");
        }
    }

    /// <summary>
    /// Settings
    /// </summary>
    public FilesystemProviderSettings Settings => _settings;

    /// <inheritdoc/>
    public async Task<string> GetHash(string name)
    {
        var bytes = await GetConfiguration(name);
        return Hasher.CreateHash(bytes);
    }

    /// <inheritdoc/>
    public Task Watch(Func<IEnumerable<string>, Task> onChange, CancellationToken cancellationToken = default)
    {
        _onChange = onChange;
        _watcher.EnableRaisingEvents = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public void Initialize()
    {
        _logger.LogInformation("Initializing {Name} provider with options {options}", Name, new
        {
            _settings.Path,
            _settings.SearchPattern,
            _settings.IncludeSubDirectories
        });

        if (!string.IsNullOrWhiteSpace(_settings.UserName) && !string.IsNullOrWhiteSpace(_settings.Password))
        {
            var credentials = new NetworkCredential(_settings.UserName, _settings.Password, _settings.Domain);
            var uri = new Uri(_settings.Path);
            _ = new CredentialCache
            {
                { new Uri($"{uri.Scheme}://{uri.Host}"), "Basic, credentials", credentials }
            };
        }

        _watcher = new FileSystemWatcher
        {
            Path = _settings.Path,
            Filter = _settings.SearchPattern,
            IncludeSubdirectories = _settings.IncludeSubDirectories,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
        };

        _watcher.Created += FileSystemWatcher_Changed;
        _watcher.Changed += FileSystemWatcher_Changed;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GetConfiguration(string name)
    {
        var path = !name.StartsWith(_settings.Path)
            ? Path.Combine(_settings.Path, name)
            : name;

        if (!File.Exists(path))
        {
            _logger.LogError("File {Path} could not be found", path);
#pragma warning disable CS8603
            return null;
#pragma warning restore CS8603
        }

        return await File.ReadAllBytesAsync(path);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<string>> ListPaths()
    {
        _logger.LogDebug("Listing files in {Path}", _settings.Path);

        var searchOptions = _settings.IncludeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var files = Directory
            .EnumerateFiles(_settings.Path, _settings.SearchPattern ?? "*", searchOptions)
            .ToList();

        _logger.LogDebug("{Count} configuration files found", files.Count());

        return Task.FromResult<IEnumerable<string>>(files);
    }

    private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        _logger.LogDebug("Configuration file change detected at {fullPath}", e.FullPath);
        var fileName = GetRelativePath(e.FullPath);
        _onChange(new[] { fileName });
    }

    private string GetRelativePath(string fullPath) =>
      Path.GetRelativePath(_settings.Path, fullPath);
}