#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using IOFIle = System.IO.File;
using IODirectory = System.IO.Directory;
using IOPath = System.IO.Path;
using Oluso.Extensions;
using Oluso.Helpers;

namespace Oluso;

/// <summary>
/// Helper class to execute validation of many different situations
/// </summary>
public static class Is
{
    #region Nullability checks

    /// <summary>
    /// checks if values are null or empty returns true if any it is
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool NullOrEmpty(params object?[] values)
    {
        if (values == null || !values.Any()) return true;
        var fails = values.Where(value =>
        {
            if (value == null) return true;

            if (value.IsInstanceOf(typeof(string)))
                return string.IsNullOrEmpty((string)value);

            switch (value)
            {
                case string s when s.Length < 1:
                case Array a when a.Length < 1:
                case ICollection c when c.Count < 1:    
                case IEnumerable e when e.Cast<object>().Any():
                    return true;
                default:
                    return false;
            }
        });
        return fails.Any();
    }

    /// <summary>
    /// checks if any of provided values is null or contains any null itam
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool NullOrAnyNull(params object?[] values)
    {
        var fails = values.Where(value =>
        {
            if (value is null) return true;

            switch (value)
            {
                case string s when s.Length < 1:
                case Array a when a.Length < 1:
                case ICollection c when c.Count < 1:
                case IEnumerable e when e.Cast<object>().Any():
                    return true;
                default:
                    return NullOrEmpty(value);
            }
        });
        return values.Any();
    }

    /// <summary>
    /// check all provided values are null
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool NotNullOrAnyNotNull(params object?[] values) =>
        !NullOrAnyNull(values);

    /// <summary>
    /// checks if source is a nullable type
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static bool IsNullable<TSource>(this TSource source)
    {
        if (source == null) return true;
        Type type = typeof(TSource);
        if (!type.IsValueType) return true;
        if (Nullable.GetUnderlyingType(type) != null) return true;
        return false;
    }

    /// <summary>
    /// check if source is null or empty if it is is Throws and <see cref="ApplicationException"/>, if not
    /// it return the source value
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsNullOrEmptyThrow<TSource>(this TSource source, string message) =>
        source.Throw(() => NullOrEmpty(source), message);

    /// <summary>
    /// check if source is null or empty if it is is Throws and <see cref="ApplicationException"/>, if not
    /// it return the source value
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsNullOrEmptyThrow<TSource>(this TSource source) =>
        source.Throw(() => NullOrEmpty(source), Messages.NullOrEmpty(nameof(source)));

    /// <summary>
    /// check that source or any of his members are not null, otherwise throws an exception 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TSource> IsNullOrAnyNullThrow<TSource>(this IEnumerable<TSource> source, string message) =>
        source.Throw(() => NullOrAnyNull(source), message);


    /// <summary>
    /// check that source or any of his members are not null, otherwise throws an exception 
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TSource> IsNullOrAnyNullThrow<TSource>(this IEnumerable<TSource> source) =>
        // ReSharper disable once PossibleMultipleEnumeration
        source.Throw(() => NullOrAnyNull(source), Messages.NullOrAnyNull(nameof(source)));
    
    /// <summary>
    /// check if any of provided values is null or has a null value, if true it will
    /// launch an exception with the provided message.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsNullOrAnyNull<TSource>(this TSource source, string message) =>
        source.Throw(() => NullOrAnyNull(source!), message);
    
    /// <summary>
    /// check if any of provided values is null or has a null value, if true it will
    /// launch an exception with the provided message.
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsNullOrAnyNull<TSource>(this TSource source) =>
        source.Throw(() => NullOrAnyNull(source!), Messages.NullOrAnyNull(nameof(source)));

    #endregion
    
    #region Checks
    
    /// <summary>
    /// checks if provided values are greater than max
    /// </summary>
    public static bool Greater(long max, params object?[] values)
    {
        var fails = values.Where(value =>
        {
            if (Is.NullOrEmpty(value!))
                return true;

            switch (value)
            {
                case Enum en when (int)value > max:
                case string s when s.Length > max:
                case sbyte sb when sb > max:
                case short sh when sh > max:
                case int inte when inte > max:
                case long lo when lo > max:
                case byte by when by > max:
                case ushort us when us > max:
                case uint ui when ui > max:
                case ulong ul when Convert.ToInt64(ul) > max:
                case char ch when ch > max:
                case float fl when fl > max:
                case double d when d > max:
                case decimal de when de > max:
                case Array a when a.Length > max:
                case ICollection c when c.Count > max:
                case IEnumerable e when e.Cast<object>().Count() > max:
                    return true;
                default:
                    return false;
            }
        });

        return fails.Any();
    }

    /// <summary>
    /// check if values are lower than min
    /// </summary>
    public static bool Lower(long min, params object?[] values)
    {
        var fails = values.Where(value =>
        {
            if (Is.NullOrEmpty(value!))
                return true;


            switch (value)
            {
                case Enum en when (int)value < min:
                case string s when s.Length < min:
                case sbyte sb when sb < min:
                case short sh when sh < min:
                case int inte when inte < min:
                case long lo when lo < min:
                case byte by when by < min:
                case ushort us when us < min:
                case uint ui when ui < min:
                case ulong ul when Convert.ToInt64(ul) < min:
                case char ch when ch > min:
                case float fl when fl < min:
                case double d when d < min:
                case decimal de when de < min:
                case Array a when a.Length < min:
                case ICollection c when c.Count < min:
                case IEnumerable e when e.Cast<object>().Count() < min:
                    return true;
                default:
                    return false;
            }
        });

        return fails.Any();
    }

    /// <summary>
    /// Check if all value(s) are equal
    /// </summary>
    /// <param name="value"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool Equal(object? value, params object?[] values)
    {
        var fails = values.Where(val => NullOrEmpty(val!) || Equals(val, value));

        return fails.Any();
    }

    /// <summary>
    /// Check if value(s) are in a given range
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool InRange(long min, long max, params object?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value!) ||
            Lower(min, value!) ||
            Greater(max, value!));

        return fails.Any();
    }

    /// <summary>
    /// Executes a RegExp over value(s)
    /// </summary>
    /// <param name="regex"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool RegexMatch(string regex, params object?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value!) ||
            !Regex.IsMatch((string)value!, regex, RegexOptions.CultureInvariant));

        return fails.Any();
    }

    /// <summary>
    /// Check if result of every Func{bool} are false
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool Custom(params Func<bool>[] values)
    {
        var fails = values.Select(f => f.Invoke()).Where(r => r);

        return !fails.Any();
    }

    /// <summary>
    /// Check if value(s) are valid names;
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool Name(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value!) ||
            RegexMatch(Regexp.ForName, value));

        return !fails.Any();
    }

    /// <summary>
    /// Check if value(s) are emails
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool Email(params string?[] values)
    {
        var fails = values.Where(value =>
        {
            if (string.IsNullOrWhiteSpace(value!) || RegexMatch(Regexp.ForEmail, value))
            {
                return true;
            }

            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(value);

                return mailAddress?.Address != value;
            }
            catch (FormatException)
            {
                return true;
            }
        });

        return !fails.Any();
    }

    /// <summary>
    /// check if value(s) are existing directories
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool Directory(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value!) ||
            !IODirectory.Exists(value));

        return !fails.Any();
    }

    /// <summary>
    /// Check if vallue(s) are existing files
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool File(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value!) ||
            !IOFIle.Exists(value));

        return !fails.Any();
    }

    /// <summary>
    /// Check if value(s) are/has extensions
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool FileExtension(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(IOPath.GetExtension(value)));

        return !fails.Any();
    }

    /// <summary>
    /// Check if value(s) is a valid domain name
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool Domain(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value!) ||
            RegexMatch(Regexp.ForDomain, value));

        return fails.Any();

    }

    /// <summary>
    /// Check if value(s) is a valid sub domain name
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool SubDomain(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value!) ||
            RegexMatch(Regexp.ForSubdomain, value));

        return fails.Any();
    }

    /// <summary>
    /// Check if value(s) are valid hostnames
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool HostName(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value!) ||
            RegexMatch(Regexp.ForHostname, value));

        return fails.Any();
    }

    /// <summary>
    /// check if value(s) are digits
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool Digit(params string?[] values)
    {
        var fails = values.Where(value =>
            string.IsNullOrWhiteSpace(value));

        return fails.Any();
    }

    /// <summary>
    /// check if value(s) has only unique chars
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool UniqueChars(params string?[] values)
    {
        var fails = values.IsNullOrAnyNull().Where(value =>
        {
            var set = new HashSet<char>();

            return string.IsNullOrEmpty(value) || value.Any(x => !set.Add(x));
        });

        return fails.Any();
    }

    /// <summary>
    /// check if value(s) has special chars
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool SpecialChars(params string?[] values)
    {
        var fails = values.Where(value =>
            string.IsNullOrWhiteSpace(value) || value.All(char.IsLetterOrDigit));

        return fails.Any();
    }

    /// <summary>
    /// check if value(s) has any lower case char
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool LowerCase(params string?[] values)
    {
        var fails = values.Where(value =>
            string.IsNullOrWhiteSpace(value) || value.All(char.IsLower));

        return fails.Any();
    }

    /// <summary>
    /// check if value(s) has any upper case char
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool UpperCase(params string?[] values)
    {
        var fails = values.Where(value =>
            string.IsNullOrWhiteSpace(value) || value.All(char.IsUpper));

        return fails.Any();
    }

    /// <summary>
    /// check if source is greater that provided max if true it throws and exception with
    /// provided message otherwise returns the same value
    /// </summary>
    /// <param name="source"></param>
    /// <param name="max"></param>
    /// <param name="message"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsGreater<TSource>(this TSource source, long max, string message) =>
        source.Throw(() => Is.Greater(max, source), message);

    /// <summary>
    /// check if source is greater than provided max if true throws an exception, otherwise
    /// returns the same value;
    /// </summary>
    /// <param name="source"></param>
    /// <param name="max"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsGreater<TSource>(this TSource source, long max) =>
        source.IsGreater(max, Messages.Greater(nameof(source), max));

    /// <summary>
    /// check if source is lower than provided min if true throws an exception with
    /// provided message, otherwise
    /// returns the same value
    /// </summary>
    /// <param name="source"></param>
    /// <param name="min"></param>
    /// <param name="message">message to display if is lower</param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsLower<TSource>(this TSource source, long min, string message) =>
        source.Throw(() => Is.Lower(min, source), message);

    /// <summary>
    /// check if source is lower than provided min if true throws and exception,otherwise
    /// returns the samevalue
    /// </summary>
    /// <param name="source"></param>
    /// <param name="min"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsLower<TSource>(this TSource source, long min) =>
        source.IsLower(min, Messages.Lower(nameof(source), min));

    /// <summary>
    /// check if source if Equal to compare value if true it throws an exception with provided
    /// message otherwise it returns the same value
    /// </summary>
    /// <param name="source"></param>
    /// <param name="compare"></param>
    /// <param name="message"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsEqual<TSource>(this TSource source, TSource compare, string message) =>
        source.Throw(() => Is.Equal(compare, source), message);

    /// <summary>
    /// Check if source is in range min and max, if true throws an exception with provided message,
    /// otherwise it returns the same value
    /// </summary>
    /// <param name="source"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="message"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsInRange<TSource>(this TSource source, long min, long max, string message) =>
        source.Throw(() => Is.InRange(min, max, source), message);

    /// <summary>
    /// check if source is in range min and max. if true throws and exception, otherwise
    /// it returns the same value.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource IsInRange<TSource>(this TSource source, long min, long max) =>
        source.IsInRange(min, max, Messages.IsInRange(nameof(source), min, max));

    /// <summary>
    /// check if the source value match the provided Regular Expression if not it throws an exception
    /// with provided message, otherwise returns the same value.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="regExp"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsRegexpMatch(this string? source, string regExp, string message) =>
        source.Throw(() => !Is.RegexMatch(regExp, source), message);

    /// <summary>
    /// check if the source value match the provided Regular Expression if not it throws an exception
    /// with provided message, otherwise returns the same value.
    /// </summary>
    public static string? IsRegexpMatch(this string? source, string regExp) =>
        source.IsRegexpMatch(regExp, Messages.InvalidFormat(nameof(source)));

    /// <summary>
    /// check if source is a valid name
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsName(this string? source, string message) =>
        source.Throw(() => !Is.Name(source), message);

    /// <summary>
    /// check if source is a valid name
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsName(this string? source) => source.IsName(Messages.InvalidFormat(nameof(source)));

    /// <summary>
    /// check if  is a valid email address if true returns email otherwise throws and exception with
    /// provided message
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string IsEmail(this string? source, string message) =>
        source.Throw(() => !Is.Email(source), message)!;

    /// <summary>
    /// check if is a valid email address if true returns the address otherwise throws and exception
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string IsEmail(this string? source) => source.IsEmail(Messages.InvalidFormat(nameof(source)));

    /// <summary>
    /// check if is a directory and exists, if true returns the same string. otherwise throws an
    /// exception with provided message
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsDirectory(this string? source, string message) =>
        source.Throw(() => !Is.Directory(source), message);

    /// <summary>
    /// check if is a directory and exists, if true returns the same string. otherwise throws
    /// an exception with message not found.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsDirectory(this string? source) =>
        source.IsDirectory(Messages.NotFound(nameof(source)));

    /// <summary>
    /// check if is a file and exists, if not throws an exception with provided message otherwise
    /// returns the same string
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsFile(this string? source, string message) =>
        source.Throw(() => !Is.File(source), message);

    /// <summary>
    /// check if is a file and exists, if not throws an exception.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsFile(this string? source) => source.IsFile(Messages.NotFound(source ?? "File"));

    /// <summary>
    /// check if is a file extension that exists if not throws an exception with provided message
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsFileExtension(this string? source, string message) =>
        source.Throw(() => !Is.FileExtension(source), message);

    /// <summary>
    /// check if is a file extension that exists if not throws an exception
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsFileExtension(this string? source) =>
        source.IsFileExtension(Messages.InvalidFormat(nameof(source)));

    /// <summary>
    /// check if is a valid sub domain if not throws an exception with provided message
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsSubDomain(this string? source, string message) =>
        source.Throw(() => !Is.SubDomain(source), message);

    /// <summary>
    /// check if is a valid sub domain if not throws an exception
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsSubDomain(this string? source) =>
        source.IsSubDomain(Messages.InvalidFormat(nameof(source)));

    /// <summary>
    /// check if is a valid domain if not throws an exception with provided message
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsDomain(this string? source, string message) =>
        source.Throw(() => !Is.Domain(source), message);

    /// <summary>
    /// check if is a valid domain if not throws and exception
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsDomain(this string? source) =>
        source.IsDomain(Messages.InvalidFormat(nameof(source)));

    /// <summary>
    /// check if is a valid host name if not it throws an exception with provided message
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsHostName(this string? source, string message) =>
        source.Throw(() => !Is.HostName(source), message);

    /// <summary>
    /// check if is a valid host name if not it throws an exception
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsHostName(this string? source) =>
        source.IsHostName(Messages.InvalidFormat(nameof(source)));

    /// <summary>
    /// check if is a valid digit if not it throws an exception with provided message
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsDigit(this string? source, string message) =>
        source.Throw(() => !Is.Digit(source), message);

    /// <summary>
    /// check if is a valid digit if not it throws an exception
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsDigit(this string? source) =>
        source.IsDigit(Messages.InvalidFormat(nameof(source)));

    /// <summary>
    /// check if it has unique chars if not throws an exception with provided message
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsUniqueChar(this string? source, string message) =>
        source.Throw(() => !Is.UniqueChars(source), message);

    /// <summary>
    /// check if it has unique chars if not throws an exception
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsUniqueChar(this string? source) =>
        source.IsUniqueChar(Messages.InvalidFormat(nameof(source)));

    /// <summary>
    /// check if it has a lower case char if not throws an exception with provided message
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsLowerCase(this string? source, string message) =>
        source.Throw(() => !Is.LowerCase(source), message);

    /// <summary>
    /// check if it has a lower case char if not throws an exception
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsLowerCase(this string? source) =>
        source.IsLowerCase(Messages.InvalidFormat(nameof(source)));

    /// <summary>
    /// check if it has a upper case char if not throws an exception with provided message
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? IsUpperCase(this string? source, string message) =>
        source.Throw(() => !Is.UpperCase(source), message);

    /// <summary>
    /// check if it has a upper case char if not throws an exception
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? IsUpperCase(this string? source) =>
        source.IsUpperCase(Messages.InvalidFormat(nameof(source)));
    
    #endregion

    /// <summary>
    /// throw an <see cref="ApplicationException"/> with provided message if func is true if no
    /// errors found or func is false it returns the original source value.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="func">validation function</param>
    /// <param name="message">message to display</param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource Throw<TSource>(this TSource source, Func<bool> func, string message)
    {
        if(source.Errors(func))
            Insist.Throw<ApplicationException>(message);
        return source;
    }

    /// <summary>
    /// check for possible errors in the source, errors could be:<br/>
    /// <li>source to be null</li>
    /// <li>func to be null</li>
    /// <li>result of the validation function return true</li>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="func">validation function to execute</param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static bool Errors<TSource>(this TSource source, Func<bool> func) =>
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        source == null || func == null || func();
}