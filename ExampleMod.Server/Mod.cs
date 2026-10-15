using Microsoft.Extensions.Logging;
using ReadyM.SDK.Attributes;

namespace ExampleMod.Server;

// The server half's entry point. As on the client, services and RPC handlers are found on their own.
[ModEntry]
public sealed partial class Mod(ILogger logger)
{
    // Runs once when the mod loads.
    private void Init()
    {
        logger.LogInformation("Example server mod loaded");
    }
}
