#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Oluso.Extensions;

/// <summary>
/// HttpContext related extensions
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// returns the {HttpRequest} ip6
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static bool ToTryIpv6(this HttpContext context, out string? ip)
    {
        ip = context.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(context))).ToTryIpv6();
        return !Is.NullOrEmpty(ip);
    }

    /// <summary>
    /// returns the {HttpRequest} ip6
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string? ToTryIpv6(this HttpContext context) =>
        context.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(context))).Connection?.RemoteIpAddress?.MapToIPv6()
            ?.ToString();
    
    /// <summary>
    /// returns the {HttpRequest} ip4
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static bool ToTryIpv4(this HttpContext context, out string? ip)
    {
        ip = context.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(context))).ToTryIpv4();
        return !Is.NullOrEmpty(ip);
    }

    /// <summary>
    /// returns the {HttpRequest} ip4
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string? ToTryIpv4(this HttpContext context) =>
        context.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(context))).Connection?.RemoteIpAddress?.MapToIPv4()
            ?.ToString();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="optionalData"></param>
    /// <returns></returns>
    public static string ToDetails(this HttpContext context,
        IDictionary<string, IEnumerable<string>>? optionalData = null)
    {
        var validHttpContext = context.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(context)));
        var sb = new StringBuilder();
        if (validHttpContext.ToTryIpv6(out var ip))
        {
            sb.Append($"IP: {ip} |");
        }

        sb.Append($" UserAgentL [{validHttpContext.User.ToUserName() ?? Environment.UserName ?? "Anonymous"} |");

        if (!Is.NullOrEmpty(validHttpContext.Request))
        {
            if (validHttpContext.Request.ToTryHeader("User-Agent", out var userAgents))
            {
                sb.Append($" UserAgent: [{userAgents?.ToString(", ")}] |");
            }

            if (validHttpContext.Request.ToTryContentType(out var contentType))
            {
                sb.Append($" ContentType: {contentType} |");
            }

            sb.Append($" {validHttpContext.Request.Method}: {validHttpContext.Request.ToPath()}");
        }

        if (!Is.NullOrEmpty(optionalData))
        {
            foreach (var data in optionalData!)
            {
                sb.Append($" {data.Key}: [{data.Value.ToString(", ")}] |");
            }    
        }
        
        return sb.ToString();
    }
}