using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Offscreen.Api;

public static class ApiEndpoints
{
    private static readonly string Version =
        typeof(ApiEndpoints).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? "unknown";

    public static IEndpointRouteBuilder MapOffscreenApi(this IEndpointRouteBuilder endpoints)
    {
        var v1 = endpoints.MapGroup("/api/v1");
        v1.MapGet("/health", () => new HealthResponse("ok", Version));
        return endpoints;
    }
}
