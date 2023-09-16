namespace Oluso.Notifier;

/// <summary>
///  different message level, it used to set how to handle
///  the message, in back and front
/// </summary>
public enum MessageLevel : byte
{
    /// <summary>
    /// Debugging message
    /// </summary>
    Debug = 2,

    /// <summary>
    /// simple information to inform to user, about current step and all that
    /// </summary>
    Information = 4,

    /// <summary>
    /// Warning message that could be handle as a minor error or so
    /// </summary>
    Warning = 8,

    /// <summary>
    /// Error that could or not affect the import/export process how the system
    /// react to it should be set by the message handler.
    /// </summary>
    Error = 16,
    
    /// <summary>
    /// allow to send data to the client
    /// </summary>
    Data = 32
}