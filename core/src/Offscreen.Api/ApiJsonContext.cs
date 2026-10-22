using System.Text.Json;
using System.Text.Json.Serialization;

namespace Offscreen.Api;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(HealthResponse))]
internal sealed partial class ApiJsonContext : JsonSerializerContext;
