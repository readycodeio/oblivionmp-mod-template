using Microsoft.Extensions.Logging;
using ReadyM.SDK.Attributes;

namespace ExampleMod.Client;

// The mod's entry point, one per half. Classes marked [Service] or [RpcHandlersFor] are found and
// created on their own, so this only needs to exist. Ask for what you need in the constructor.
[ModEntry]
public sealed partial class Mod(ILogger logger)
{
    // Runs once when the mod loads.
    private void Init()
    {
        logger.LogInformation("Example mod loaded. Alt+N adds gold, Alt+B shows your balance, Alt+P pings nearby players.");
    }
}
