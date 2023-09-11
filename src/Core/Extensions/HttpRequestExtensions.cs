#nullable enable
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Oluso.Extensions;

/// <summary>
/// HttpRequest related extensions
/// </summary>
public static class HttpRequestExtensions
{
    /// <summary>
    /// returns a header value from an {HttpRequest}
    /// </summary>
    /// <param name="httpRequest"></param>
    /// <param name="key"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ToTryHeader(this HttpRequest httpRequest, string key, out List<string>? values)
    {
        var validHttpRequest = httpRequest.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(httpRequest)));
        key.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(key)));

        values = validHttpRequest.ToHeader(key);

        return !Is.NullOrAnyNull(values);
    }
    
    /// <summary>
    /// returns the header valid of a valid {HttpRequest}
    /// </summary>
    /// <param name="httpRequest"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static List<string>? ToHeader(this HttpRequest httpRequest, string key)
    {
        var validHttpRequest = httpRequest.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(httpRequest)));
        key.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(key)));

        return validHttpRequest.Headers.ContainsKey(key) ? validHttpRequest.Headers[key].ToList() : null;
    }

    /// <summary>
    /// return the Url of a {HttpRequest}
    /// </summary>
    /// <param name="httpRequest"></param>
    /// <returns></returns>
    public static string ToPath(this HttpRequest httpRequest) =>
        $"{httpRequest?.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(httpRequest))).Scheme}://{httpRequest?.Host.Value}{httpRequest?.Path.Value}{httpRequest?.QueryString.Value}";

    /// <summary>
    /// returns the hostname or subdomain of a {HttpRequest}
    /// </summary>
    /// <param name="httpRequest"></param>
    /// <returns></returns>
    public static string? ToSubdomain(this HttpRequest httpRequest) =>
        httpRequest.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(httpRequest)))
            .Host.Host?.Split('.').ToString().Trim();

    /// <summary>
    /// gets the {HttpRequest} ContentType
    /// </summary>
    /// <param name="httpRequest"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public static bool ToTryContentType(this HttpRequest httpRequest, out string? contentType)
    {
        contentType = httpRequest.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(httpRequest))).ContentType;
        return !contentType.IsNullOrEmpty();
    }
}