using ExampleMod.Common;
using Microsoft.Extensions.Logging;
using ReadyM.Relay.Server.Sdk.Rpc;
using ReadyM.SDK.Archetypes.Core;
using ReadyM.SDK.Attributes;
using ReadyM.SDK.Server.Entities;

namespace ExampleMod.Server;

// The server end of RpcContracts. Each handler gets an RpcContext saying who sent it.
[RpcHandlersFor(typeof(RpcContracts))]
public sealed partial class RpcHandlers(IEntities entities, ILogger logger)
{
    private const int Reward = 100;

    // Credit the sender's wallet and reply with the amount added.
    partial void OnAddWalletBalance(RpcContext context)
    {
        if (!entities.TryLookup(context.Sender, out Player player))
            return;

        player.Balance += Reward;
        logger.LogInformation("Added {Reward} gold for player {Player}. Balance: {Balance}",
            Reward, context.Sender, player.Balance);

        SendAddWalletBalance(context.Sender, Reward);
    }

    // Reply with the sender's balance.
    partial void OnGetWalletBalance(RpcContext context)
    {
        if (entities.TryLookup(context.Sender, out Player player))
            SendGetWalletBalance(context.Sender, player.Balance);
    }
}
