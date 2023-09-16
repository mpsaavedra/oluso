using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthCheckOptions = Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions;

namespace Oluso.HealthCheck.Extensions;

/// <summary>
/// Endpoint route builder extensions
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// map health checking endpoints 
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="pattern">path where to map health checking to in the API</param>
    public static void MapHealthChecking(
        this IEndpointRouteBuilder endpoint, string pattern = "/health/")
    {
        endpoint.MapHealthChecks($"{pattern}ready", new HealthCheckOptions()
        {
            Predicate = (Func<HealthCheckRegistration, bool>)(check => check.Tags.Contains("ready"))
        });
        endpoint.MapHealthChecks($"{pattern}live", new HealthCheckOptions()
        {
            Predicate = (Func<HealthCheckRegistration, bool>)(_ => false)
        });
        endpoint.MapHealthChecks($"{pattern}details", new HealthCheckOptions()
        {
            ResponseWriter = async (context, report) =>
            {
                var q = new
                {
                    status = report.Status.ToString(),
                    monitors = report.Entries.Select(e => new
                    {
                        key = e.Key,
                        value = Enum.GetName(typeof(HealthStatus), (object)e.Value.Status)
                    })
                };
                string result = System.Text.Json.JsonSerializer.Serialize(q);
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
        });
    }
}