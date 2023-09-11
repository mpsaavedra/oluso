#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Oluso.Extensions;

/// <summary>
/// <see cref="HttpResponse"/> extensions
/// </summary>
public static class HttpResponseExtensions
{
    /// <summary>
    /// response to an Http request through am {HttpResponse}
    /// </summary>
    /// <param name="httpResponse">response side of the Http request</param>
    /// <param name="status">response status</param>
    /// <param name="message">response message</param>
    /// <param name="contentType">response content type</param>
    /// <param name="headers">response headers</param>
    /// <returns>Http response | Exception if httpResponse is null</returns>
    public static async Task ToResultAsync(
        this HttpResponse httpResponse,
        HttpStatusCode status,
        string message,
        // string contentType = MediaTypeNames.Application.Json,
        string contentType = "Json",
        IDictionary<string, IEnumerable<string>>? headers = null)
    {
        var validHttpResponse = httpResponse.IsNullOrEmptyThrow(Messages.NullOrEmpty(nameof(httpResponse)));
        validHttpResponse.Clear();
        validHttpResponse.StatusCode = (int)status;
        validHttpResponse.ContentType = contentType;

        if (headers?.Count > 0)
        {
            foreach (var header in headers)
            {
                validHttpResponse.Headers.AppendList(header.Key, header.Value.ToList());
            }            
        }
        
        await httpResponse.WriteAsync(message).ConfigureAwait(false);
    }
}