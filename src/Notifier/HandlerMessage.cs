namespace Oluso.Notifier;

/// <summary>
/// Message to be send to the client. This message has the intention to be a singleton
/// object 
/// </summary>
public class HandlerMessage
{
    private readonly SortedDictionary<DateTime, MessageData> _messages = new();
    private readonly Exception? _exception = null;
#pragma warning disable CS8618
    private static HandlerMessage _instance;
#pragma warning restore CS8618

    /// <summary>
    /// return the singleton instance of the HandlerMessage 
    /// </summary>
    public static HandlerMessage Instance => _instance ??= new HandlerMessage();

    /// <summary>
    /// returns a new <see cref="HandlerMessage"/> instance
    /// </summary>
    /// <param name="data"></param>
    /// <param name="status"></param>
    /// <param name="level"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    public static HandlerMessage New(object? data = null, ProcessStatus? status = null,
        MessageLevel level = MessageLevel.Information, Exception? exception = null)
    {
        var result = new HandlerMessage
        {
            LaunchTime = DateTime.UtcNow,
            Data = data,
            Status = status ?? ProcessStatus.Initializing
        };

        if (exception is null)
        {
            result.InnerException = exception;
            result.MessageLevel = MessageLevel.Error;
        }
        else
        {
            result.MessageLevel = level;
        }

        return result;
    }

    /// <summary>
    /// returns a new Information message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static HandlerMessage NewInformationMessage(string message) => 
        New(message, level: MessageLevel.Information);

    /// <summary>
    /// returns a new Debug message
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static HandlerMessage NewDebugMessage(object? data = null) => 
        New(data, level: MessageLevel.Information);

    /// <summary>
    /// returns a new Data message
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static HandlerMessage NewDataMessage(object? data = null) => 
        New(data, level: MessageLevel.Information);
    
    /// <summary>
    /// returns a new Error message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    public static HandlerMessage NewErrorMessage(string message, Exception? exception = null) => 
        New(message, level: MessageLevel.Error, exception: exception);

    /// <summary>
    /// returns the last message
    /// </summary>
    public MessageData Message
    {
        get => _messages.Last().Value;
        set
        {
            if(!_messages.ContainsKey(value.CreationDate))
                _messages.Add(DateTime.UtcNow, value);
        }
    }

    /// <summary>
    /// get the last error message or adds a new error message
    /// </summary>
    public MessageData ErrorMessage
    {
        get => _messages.Last(x => x.Value.Level == MessageLevel.Error).Value;
        set
        {
            var msg = value with { CreationDate = DateTime.UtcNow, Level = MessageLevel.Error };
            _messages.Add(DateTime.UtcNow, msg);
        }
    }

    /// <summary>
    /// get the exception if any. set the exception and adds a new error message to the message list
    /// </summary>
    public Exception? InnerException
    {
        get => _exception;
        set
        {
            if (value != null)
            {
                var msg = new MessageData(DateTime.UtcNow, MessageLevel.Error, "", value);
                _messages.Add(DateTime.UtcNow, msg);
            }
        }
    }

    /// <summary>
    /// returns the list of information messages
    /// </summary>
    public List<string> InformationMessages => _messages
        .Where(x => x.Value.Level == MessageLevel.Information)
        .Select(x => x.Value.Data)
        .ToList();

    /// <summary>
    /// returns the list of warning messages
    /// </summary>
    public List<string> WarningMessages => _messages
        .Where(x => x.Value.Level == MessageLevel.Warning)
        .Select(x => x.Value.Data)
        .ToList();

    /// <summary>
    /// returns the list of debug messages
    /// </summary>
    public List<string> DebugMessages => _messages
        .Where(x => x.Value.Level == MessageLevel.Debug)
        .Select(x => x.Value.Data)
        .ToList();

    /// <summary>
    /// returns the list of debug messages
    /// </summary>
    public List<string> ErrorMessages => _messages
        .Where(x => x.Value.Level == MessageLevel.Error)
        .Select(x => x.Value.Data + $" {x.Value.Exception?.Message}")
        .ToList();

    /// <summary>
    /// return all messages
    /// </summary>
    public List<string> AllMessages => _messages
        .Select(x =>
        {
            var result = $"{x.Value.Level.ToString()}: ";
            if (x.Value.Level == MessageLevel.Error)
                result += x.Value.Data + $" {x.Value.Exception?.Message}";
            result += x.Value.Data;
            
            return result;
        })
        .ToList();
    
    /// <summary>
    /// define the process status
    /// </summary>
    public ProcessStatus Status { get; set; } = ProcessStatus.NotAvailable;
    
    /// <summary>
    /// Date and time the notification was launch
    /// </summary>
    public DateTime LaunchTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// data to be send among the message
    /// </summary>
    public object? Data { get; set; } = null;

    /// <summary>
    /// message level to represent the message type
    /// </summary>
    public MessageLevel MessageLevel { get; set; } = MessageLevel.Information;

    /// <summary>
    /// register the message into the message instance
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public HandlerMessage FromMessage(HandlerMessage? message = null)
    {
        message ??= new HandlerMessage();
        Status = message.Status;
        LaunchTime = message.LaunchTime;
        Data = message.Data;
        MessageLevel = message.MessageLevel;
        InnerException = message.InnerException;
        Message = message.Message;
        return this;
    }
}