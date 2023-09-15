using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Oluso.Configuration.Client.Parsers;

/// <summary>
/// Json file configuration parser. it parse the configuration stream assuming that
/// the stream contains an json file format and returns the <see cref="IDictionary{TKey, TValue}"/>
/// representation of the configuration.
/// </summary>
public class JsonConfigurationFileParser : IConfigurationParser
{
    private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    private readonly Stack<string> _context = new Stack<string>();

#pragma warning disable CS8618
    private string _currentPath;
#pragma warning restore CS8618

    /// <inheritdoc/>
    public IDictionary<string, string> Parse(Stream input) => ParseStream(input);

    private IDictionary<string, string> ParseStream(Stream input)
    {
        _data.Clear();

        var jsonDocumentOptions = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        using (var reader = new StreamReader(input))
        using (JsonDocument doc = JsonDocument.Parse(reader.ReadToEnd(), jsonDocumentOptions))
        {
            if (doc.RootElement.ValueKind != JsonValueKind.Object)
            {
                throw new FormatException($"Unsupported JSON token '{doc.RootElement.ValueKind}' was found.");
            }
            VisitElement(doc.RootElement);
        }

        return _data;
    }

    private void VisitElement(JsonElement element)
    {
        foreach (var property in element.EnumerateObject())
        {
            EnterContext(property.Name);
            VisitValue(property.Value);
            ExitContext();
        }
    }

    private void VisitValue(JsonElement value)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.Object:
                VisitElement(value);
                break;

            case JsonValueKind.Array:
                var index = 0;
                foreach (var arrayElement in value.EnumerateArray())
                {
                    EnterContext(index.ToString());
                    VisitValue(arrayElement);
                    ExitContext();
                    index++;
                }
                break;

            case JsonValueKind.Number:
            case JsonValueKind.String:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
                var key = _currentPath;
                if (_data.ContainsKey(key))
                {
                    throw new FormatException($"A duplicate key '{key}' was found.");
                }
                _data[key] = value.ToString();
                break;

            default:
                throw new FormatException($"Unsupported JSON token '{value.ValueKind}' was found.");
        }
    }

    private void EnterContext(string context)
    {
        _context.Push(context);
        _currentPath = ConfigurationPath.Combine(_context.Reverse());
    }

    private void ExitContext()
    {
        _context.Pop();
        _currentPath = ConfigurationPath.Combine(_context.Reverse());
    }
}