using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Oluso.Exceptions;

namespace Oluso;

/// <summary>
/// 
/// </summary>
public static class Insist
{
    /// <summary>
    /// 
    /// </summary>
    public static class MustBe
    {
        /// <summary>
        /// check that item is not null or whitespace if throw an exception 
        /// </summary>
        /// <param name="item"></param>
        public static void NotNullOrWhiteSpace(string item)
        {
            if(string.IsNullOrWhiteSpace(item) || string.IsNullOrEmpty(item))
                Throw<ItemNullException>(item);
        }
        
        /// <summary>
        /// check that item is not null or empty if throw an exception 
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="TItem"></typeparam>
        public static void NotNull<TItem>(TItem item)
        {
            if (item is null)
            {
                Throw<ItemNullException>(nameof(item));
            }
        }
        
        /// <summary>
        /// check that condition is true if not throw an exception , it gets the message from the provided function.
        ///
        ///
        /// <code>
        /// Insist.MustBe.True(condition, () => "This is the message if not true");
        /// Insist.MustBe.True(condition, () =>
        /// {
        ///     // do something
        ///     return "This is the message if condition is not true";
        /// });
        /// </code>
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="msg"></param>
        /// <typeparam name="TException"></typeparam>
        public static void True<TException>(bool condition, Func<string> msg) where TException : Exception
        {
            if (!condition)
                Throw<TException>(msg());
        }

        /// <summary>
        /// check that is true if not throw an exception  with provided message
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="msg"></param>
        /// <typeparam name="TException"></typeparam>
        public static void True<TException>(bool condition, string msg) where TException : Exception =>
            True<TException>(condition, () => msg);

        /// <summary>
        /// check that is true throw an exception , it gets the message from the provided function.
        ///
        ///
        /// <code>
        /// Insist.MustBe.False(condition, () => "This is the message if false");
        /// Insist.MustBe.False(condition, () =>
        /// {
        ///     // do something
        ///     return "This is the message if condition is false";
        /// });
        /// </code>
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="msg"></param>
        /// <typeparam name="TException"></typeparam>
        public static void False<TException>(bool condition, Func<string> msg) where TException : Exception =>
            True<TException>(!condition, msg);

        /// <summary>
        /// check that condition is true if not throw an exception 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="msg"></param>
        /// <typeparam name="TException"></typeparam>
        public static void False<TException>(bool condition, string msg) where TException : Exception =>
            False<TException>(condition, () => msg);
    }

    private static readonly object LockObject = new();
    private static Dictionary<Type, ConstructorInfo> EncounteredExceptionTypes { get; set; } = new();
    
    /// <summary>
    /// gets the types of all launched exception
    /// </summary>
    public static IEnumerable<Type> CachedExceptionTypes
    {
        get
        {
            lock (LockObject)
            {
                return EncounteredExceptionTypes.Keys.ToArray();
            }
        }
    }

    /// <summary>
    /// Throw the specified 
    /// </summary>
    /// <param name="msg"></param>
    /// <typeparam name="TException"></typeparam>
    public static void Throw<TException>(string msg) where TException : Exception
    {
        ConstructorInfo info;

        lock (LockObject)
        {
            var t = typeof(TException);
            if (EncounteredExceptionTypes.ContainsKey(t))
            {
                info = EncounteredExceptionTypes[t];
            }
            else
            {
                info = t.GetConstructor(new[] { typeof(string) })!;

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (info is not null)
                {
                    EncounteredExceptionTypes[t] = info;
                }
            }
            var ex = (TException)info?.Invoke(new[] { msg });
            // RegisterException<TException>(t as TException ?? default(TException), msg);
            RegisterException<TException>(ex, msg);
        }
        
        throw (info?.Invoke(new[] { msg })) as TException;
    }

    /// <summary>
    /// Throw the specified <typeparamref name="TException" /> with provided messages. Instead of launch
    /// an exception for every message it join all messages into a single message separated by comma
    /// </summary>
    /// <param name="msgs"></param>
    /// <typeparam name="TException"></typeparam>
    public static void Throw<TException>(IEnumerable<string> msgs) where TException : Exception
    {
        var messages = msgs as string[] ?? msgs.ToArray();
        if (!(messages?.Count() < 0))
        {
            return;
        }

        var errors = messages.Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).AsEnumerable();
        var error = errors.Aggregate("  ", (current, msg) => current + (msg + ", "));
        error = error?.Substring(1, error.Length - 3);
        
        Throw<TException>(error);
    }

    /// <summary>
    /// throw an exception if any of the provided messages is valid
    /// </summary>
    /// <param name="msgs"></param>
    /// <typeparam name="TException"></typeparam>
    public static void ThrowAny<TException>(params string?[] msgs) where TException : Exception
    {
        var errors = msgs.Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();

        if (!errors.Any()) return;
        
        Throw<TException>(errors!);
    }
    
    /// <summary>
    /// throw an exception if any of the provided messages is valid
    /// </summary>
    /// <param name="msgs"></param>
    /// <typeparam name="TException"></typeparam>
    public static void ThrowAny<TException>(IEnumerable<string?>? msgs) where TException : Exception
    {
        var errors = msgs.Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();
        if(errors.Any())
            Throw<TException>(errors);
    }

    /// <summary>
    /// register a launched exception into the exceptions stack
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="msg"></param>
    /// <typeparam name="TException"></typeparam>
    public static void RegisterException<TException>(TException exception, string msg)
        where TException: Exception
    {
        lock (LockObject)
        {
            var tmp = new Dictionary<Exception, string>()
            {
                { exception, msg }
            };
            Exceptions.Add(DateTimeOffset.UtcNow, tmp);
        }
    }

    /// <summary>
    /// get/set the launched exceptions
    /// </summary>
    public static SortedList<DateTimeOffset, Dictionary<Exception, string>> Exceptions { get; set; } = new();
    
}