using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Offscreen.Api;

public static class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddOffscreenApi(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.Configure<JsonOptions>(options =>
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, ApiJsonContext.Default));
        return services;
    }
}
