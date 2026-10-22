using Offscreen.Api;
using Offscreen.Host;
using Offscreen.Infrastructure.Configuration;

if (DotEnv.Find(Directory.GetCurrentDirectory()) is { } dotEnv)
{
    DotEnv.Load(dotEnv);
}

var builder = WebApplication.CreateSlimBuilder(args);
builder.Configuration.AddEnvironmentVariables("OFFSCREEN_");

builder.Logging.ClearProviders();
if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddSimpleConsole(options =>
    {
        options.SingleLine = true;
        options.TimestampFormat = "HH:mm:ss ";
    });
}
else
{
    builder.Logging.AddJsonConsole(options => options.TimestampFormat = "O");
}

var server = builder.Configuration.GetSection(ServerOptions.Section).Get<ServerOptions>() ?? new ServerOptions();
if (server.Port is < 1 or > 65535)
{
    throw new InvalidOperationException($"Server:Port must be between 1 and 65535, got {server.Port}.");
}

builder.WebHost.ConfigureKestrel(kestrel => kestrel.ListenAnyIP(server.Port));
builder.Services.AddOffscreenApi();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();
app.MapOffscreenApi();

app.Run();

public partial class Program;
