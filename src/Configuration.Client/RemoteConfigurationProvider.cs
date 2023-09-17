using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Oluso.Configuration.Client.Parsers;

namespace Oluso.Configuration.Client;

internal class RemoteConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly ILogger _logger;

    private readonly RemoteConfigurationSource _source;
    private readonly Lazy<HttpClient> _httpClient;
    private readonly IConfigurationParser? _parser;
    private bool _isDisposed;

    private string Hash { get; set; }

    private HttpClient HttpClient => _httpClient.Value;

#pragma warning disable CS8618
    /// <summary></summary>
    public RemoteConfigurationProvider(RemoteConfigurationSource source)
    {
#pragma warning restore CS8618
        _source = source ?? throw new ArgumentNullException(nameof(source));

        if (source.ConfigurationName == null)
        {
            throw new ArgumentNullException(nameof(source.ConfigurationName));
        }

        if (source.ConfigurationServiceUri == null)
        {
            throw new ArgumentNullException(nameof(source.ConfigurationServiceUri));
        }

        Logger.LoggerFactory = source.LoggerFactory ?? new NullLoggerFactory();

        _logger = Logger.CreateLogger<RemoteConfigurationProvider>();

        _logger.LogInformation("Initializing remote configuration source for configuration '{ConfigurationName}'.", source.ConfigurationName);

        _httpClient = new Lazy<HttpClient>(CreateHttpClient);

        _parser = source.Parser;

        if (_parser == null)
        {
            var extension = Path.GetExtension(source.ConfigurationName).ToLower();

            _logger.LogInformation("A file parser was not specified. Attempting to resolve parser from file extension '{extension}'.", extension);

            switch (extension)
            {
                case ".ini":
                    _parser = new IniConfigurationParser();
                    break;
                case ".xml":
                    _parser = new XmlConfigurationFileParser();
                    break;
                default:
                    _parser = new JsonConfigurationFileParser();
                    break;
            }
        }

        _logger.LogInformation("Using parser {Name}.", _parser.GetType().Name);

        if (source.ReloadOnChange)
        {
            if (source.CreateSubscriber == null)
            {
                _logger.LogWarning("ReloadOnChange is enabled but a subscriber has not been configured.");
                return;
            }

            var subscriber = source.CreateSubscriber();

            _logger.LogInformation("Initializing remote configuration {Name} subscriber for configuration '{ConfigurationName}'.", subscriber.Name, source.ConfigurationName);

            subscriber.Initialize();

            subscriber.Subscribe(source.ConfigurationName, message =>
            {
                _logger.LogInformation("Received remote configuration change subscription for configuration '{ConfigurationName}' with hash {message}. " +
                                       "Current hash is {Hash}.", source.ConfigurationName, message, Hash);

                if (message != null && message.Equals(Hash, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("Configuration '{ConfigurationName}' current hash {Hash} matches new hash. " +
                                           "Configuration will not be updated.", source.ConfigurationName, Hash);

                    return;
                }

                Load();
                OnReload();
            });
        }
    }

    public override void Load() => LoadAsync().ContinueWith(task =>
    {
        if (task.IsFaulted)
        {
            if (task.Exception != null)
            {
                var ex = task.Exception.Flatten();
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }).GetAwaiter().GetResult();

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        if (_httpClient?.IsValueCreated == true)
        {
            _httpClient.Value.Dispose();
        }

        _isDisposed = true;
    }

    private HttpClient CreateHttpClient()
    {
        var handler = _source.HttpMessageHandler ?? new HttpClientHandler();
        var client = new HttpClient(handler, true)
        {
            BaseAddress = new Uri(_source.ConfigurationServiceUri),
            Timeout = _source.RequestTimeout
        };

        return client;
    }

    private async Task LoadAsync()
    {
        Data = await RequestConfigurationAsync().ConfigureAwait(false);
    }

    private async Task<IDictionary<string, string>> RequestConfigurationAsync()
    {
        var encodedConfigurationName = WebUtility.UrlEncode(_source.ConfigurationName);

        _logger.LogInformation("Requesting remote configuration {ConfigurationName} from {BaseAddress}.", _source.ConfigurationName, HttpClient.BaseAddress);

        try
        {
            using (var response = await HttpClient.GetAsync(encodedConfigurationName).ConfigureAwait(false))
            {
                _logger.LogInformation("Received response status code {StatusCode} from endpoint for configuration '{ConfigurationName}'.",
                    response.StatusCode, _source.ConfigurationName);

                if (response.IsSuccessStatusCode)
                {
                    using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        _logger.LogInformation("Parsing remote configuration response stream ({Length:N0} bytes) for configuration '{ConfigurationName}'.",
                            stream.Length, _source.ConfigurationName);

                        Hash = ComputeHash(stream);
                        _logger.LogInformation("Computed hash for Configuration '{ConfigurationName}' is {Hash}.", _source.ConfigurationName, Hash);

                        stream.Position = 0;
                        var data = _parser?.Parse(stream);

                        _logger.LogInformation("Configuration updated for '{ConfigurationName}'.", _source.ConfigurationName);

                        return data!;
                    }
                }

                if (!_source.Optional)
                {
                    throw new Exception($"Error calling remote configuration endpoint: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
        catch (Exception)
        {
            if (!_source.Optional)
            {
                throw;
            }
        }
#pragma warning disable CS8603
        return null;
#pragma warning restore CS8603
    }

    private string ComputeHash(Stream stream)
    {
        using (var hash = SHA1.Create())
        {
            var hashBytes = hash.ComputeHash(stream);

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}