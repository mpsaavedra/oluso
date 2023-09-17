using Microsoft.Extensions.Options;

namespace Configuration.Client;

#pragma warning disable CS8618

public class ConfigWriter
{
    private readonly IOptionsMonitor<Config> _config;

    public ConfigWriter(IOptionsMonitor<Config> config)
    {
        _config = config;
    }

    public async Task Write(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var config = _config.CurrentValue;
            Console.WriteLine(config.ToString());

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }
}

public class Config
{
    public int First { get; set; }
    public string Second { get; set; }
    public string Third { get; set; }

    public override string ToString() =>
        $"Config [First: {First}, Second: {Second}, Third: {Third}]";
}
#pragma warning restore CS8618
