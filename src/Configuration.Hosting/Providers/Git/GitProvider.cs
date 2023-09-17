using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Extensions.Logging;
using Oluso.Configuration.Abstractions;
using Oluso.Configuration.Abstractions.Extensions;
using Oluso.Configuration.Abstractions.Helpers;

namespace Oluso.Configuration.Hosting.Providers.Git;

public class GitProvider : IProvider
{
    private readonly ILogger<GitProvider> _logger;
    private readonly GitProviderSettings _settings;
    private CredentialsHandler _credentialsHandler;
    public string Name => "Git provider";

    public GitProvider(ILogger<GitProvider> logger, GitProviderSettings settings)
    {
        _logger = logger;
        _settings = settings;
        settings.LocalPath.ToNullEmptyOrWhitespaceThrow(nameof(settings.LocalPath));
        settings.RepositoryUrl.ToNullEmptyOrWhitespaceThrow(nameof(settings.RepositoryUrl));
    }
    
    public async Task Watch(Func<IEnumerable<string>, Task> onChange, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                List<string> files;

                var task = Task.Run(ListChangedFiles, cancellationToken);
                // The git fetch operation can sometimes hang.  Force to complete after a minute.
                if (task.Wait(TimeSpan.FromSeconds(60)))
                {
                    files = task.Result.ToList();
                }
                else
                {
                    throw new Exception("Attempting to list changed files timed out after 60 seconds.");
                }

                if (files.Count > 0)
                {
                    await onChange(files);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while attempting to poll for changes");
            }

            var delayDate = DateTime.UtcNow.Add(_settings.PollingInterval);

            _logger.LogInformation("Next polling period will begin in {PollingInterval:c} at {delayDate}.", _settings.PollingInterval, delayDate);

            await Task.Delay(_settings.PollingInterval, cancellationToken);
        }
    }

    public void Initialize()
        {
            _logger.LogInformation("Initializing {Name} provider with options {Options}.", Name, new
            {
                _settings.RepositoryUrl,
                _settings.LocalPath,
                _settings.Branch,
                _settings.PollingInterval,
                _settings.SearchPattern
            });

            if (Directory.Exists(_settings.LocalPath))
            {
                _logger.LogInformation("A local repository already exists at {LocalPath}.", _settings.LocalPath);

                _logger.LogInformation("Deleting directory {LocalPath}.", _settings.LocalPath);

                DeleteDirectory(_settings.LocalPath);
            }

            if (!Directory.Exists(_settings.LocalPath))
            {
                _logger.LogInformation("Creating directory {LocalPath}.", _settings.LocalPath);

                Directory.CreateDirectory(_settings.LocalPath);
            }

            if (_settings.Username != null && _settings.Password != null)
            {
                _credentialsHandler = (url, user, cred) => new UsernamePasswordCredentials
                {
                    Username = _settings.Username,
                    Password = _settings.Password
                };
            }

            var cloneOptions = new CloneOptions
            {
                CredentialsProvider = _credentialsHandler,
                BranchName = _settings.Branch
            };

            _logger.LogInformation("Cloning git repository {RepositoryUrl} to {LocalPath}.", _settings.RepositoryUrl, _settings.LocalPath);

            var path = Repository.Clone(_settings.RepositoryUrl, _settings.LocalPath, cloneOptions);

            _logger.LogInformation("Repository cloned to {path}.", path);

            using var repo = new Repository(_settings.LocalPath);
            var hash = repo.Head.Tip.Sha.Substring(0, 6);

            _logger.LogInformation("Current HEAD is [{hash}] '{MessageShort}'.", hash, repo.Head.Tip.MessageShort);
        }

        public async Task<byte[]> GetConfiguration(string name)
        {
            string path = Path.Combine(_settings.LocalPath, name);

            if (!File.Exists(path))
            {
                _logger.LogInformation("File does not exist at {path}.", path);
                return null;
            }

            return await File.ReadAllBytesAsync(path);
        }

        public async Task<string> GetHash(string name)
        {
            var bytes = await GetConfiguration(name);

            return Hasher.CreateHash(bytes);
        }

        public Task<IEnumerable<string>> ListPaths()
        {
            _logger.LogInformation("Listing files at {LocalPath}.", _settings.LocalPath);

            IList<string> files = new List<string>();

            using (var repo = new Repository(_settings.LocalPath))
            {
                _logger.LogInformation("Listing files in repository at {LocalPath}.", _settings.LocalPath);

                foreach (var entry in repo.Index)
                {
                    files.Add(entry.Path);
                }
            }

            var localFiles = Directory.EnumerateFiles(_settings.LocalPath, _settings.SearchPattern ?? "*", SearchOption.AllDirectories).ToList();
            localFiles = localFiles.Select(GetRelativePath).ToList();

            files = localFiles.Intersect(files).ToList();

            _logger.LogInformation("{Count} files found.", files.Count);

            return Task.FromResult<IEnumerable<string>>(files);
        }

        private async Task<IEnumerable<string>> ListChangedFiles()
        {
            Fetch();

            IList<string> changedFiles = new List<string>();

            using (var repo = new Repository(_settings.LocalPath))
            {
                _logger.LogInformation("Checking for remote changes on {RemoteName}.", repo.Head.TrackedBranch.RemoteName);

                foreach (TreeEntryChanges entry in repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, repo.Head.TrackedBranch.Tip.Tree))
                {
                    if (entry.Exists)
                    {
                        _logger.LogInformation("File {Path} changed.", entry.Path);
                        changedFiles.Add(entry.Path);
                    }
                    else
                    {
                        _logger.LogInformation("File {Path} no longer exists.", entry.Path);
                    }
                }
            }

            if (changedFiles.Count == 0)
            {
                _logger.LogInformation("No tree entry changes were detected.");

                return changedFiles;
            }

            UpdateLocal();

            var filteredFiles = await ListPaths();
            changedFiles = filteredFiles.Intersect(changedFiles).ToList();

            _logger.LogInformation("{Count} files changed.", changedFiles.Count);

            return changedFiles;
        }

        private void UpdateLocal()
        {
            using var repo = new Repository(_settings.LocalPath);
            var options = new PullOptions
            {
                FetchOptions = new FetchOptions
                {
                    CredentialsProvider = _credentialsHandler
                }
            };

            var signature = new Signature(new Identity("Configuration Service", "Configuration Service"), DateTimeOffset.Now);

            _logger.LogInformation("Pulling changes to local repository.");

            var currentHash = repo.Head.Tip.Sha.Substring(0, 6);

            _logger.LogInformation("Current HEAD is [{currentHash}] '{MessageShort}'.", currentHash, repo.Head.Tip.MessageShort);

            var result = Commands.Pull(repo, signature, options);

            _logger.LogInformation("Merge completed with status {Status}.", result.Status);

            var newHash = result.Commit.Sha.Substring(0, 6);

            _logger.LogInformation("New HEAD is [{newHash}] '{MessageShort}'.", newHash, result.Commit.MessageShort);
        }

        private static void DeleteDirectory(string path)
        {
            foreach (var directory in Directory.EnumerateDirectories(path))
            {
                DeleteDirectory(directory);
            }

            foreach (var fileName in Directory.EnumerateFiles(path))
            {
                var fileInfo = new FileInfo(fileName)
                {
                    Attributes = FileAttributes.Normal
                };

                fileInfo.Delete();
            }

            Directory.Delete(path);
        }

        private void Fetch()
        {
            using var repo = new Repository(_settings.LocalPath);
            FetchOptions options = new FetchOptions
            {
                CredentialsProvider = _credentialsHandler
            };

            foreach (var remote in repo.Network.Remotes)
            {
                var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);

                _logger.LogInformation("Fetching from remote {Name} at {Url}.", remote.Name, remote.Url);

                Commands.Fetch(repo, remote.Name, refSpecs, options, string.Empty);
            }
        }

        private string GetRelativePath(string fullPath)
        {
            return Path.GetRelativePath(_settings.LocalPath, fullPath);
        }
}