using ExampleMod.Common;
using Microsoft.Extensions.Logging;
using OblivionMp.Sdk;
using ReadyM.Api.Idents;
using ReadyM.SDK.Attributes;

namespace ExampleMod.Client;

// The client end of RpcContracts. Implement a handler for each message the client receives; the
// Send methods for the ones it sends are generated on this class.
[RpcHandlersFor(typeof(RpcContracts))]
public sealed partial class RpcHandlers(IPlayers players, ILogger logger)
{
    partial void OnAddWalletBalance(int amount)
    {
        logger.LogInformation("The server added {Amount} gold to your wallet.", amount);
    }

    partial void OnGetWalletBalance(int amount)
    {
        logger.LogInformation("The server says your balance is {Amount} gold.", amount);

        // The wallet is replicated, so the client can also read it straight off its own player.
        if (players.LocalPlayer is { } me)
            logger.LogInformation("Your replicated balance is {Balance} gold.", me.Balance);
    }

    partial void OnPing(PlayerId sender, string message)
    {
        logger.LogInformation("Ping from player {Sender}: {Message}", sender, message);
    }
}
