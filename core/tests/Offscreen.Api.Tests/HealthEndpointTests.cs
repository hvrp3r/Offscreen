using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Offscreen.Api;

namespace Offscreen.Api.Tests;

public sealed class HealthEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Health_ReturnsStatusAndVersion()
    {
        using var client = factory.CreateClient();

        var health = await client.GetFromJsonAsync<HealthResponse>("/api/v1/health", TestContext.Current.CancellationToken);

        Assert.NotNull(health);
        Assert.Equal("ok", health.Status);
        Assert.StartsWith("0.1.0", health.Version, StringComparison.Ordinal);
    }

    [Fact]
    public async Task UnknownRoute_ReturnsProblemDetails()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/api/v1/unknown", UriKind.Relative), TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }
}
