namespace Offscreen.Host;

public sealed class ServerOptions
{
    public const string Section = "Server";

    public int Port { get; set; } = 8420;
}
