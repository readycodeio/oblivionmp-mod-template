using ExampleMod.Common;
using Microsoft.Extensions.Logging;
using ReadyM.Api.Multiplayer;
using ReadyM.Api.Multiplayer.ECS.Components;
using ReadyM.Relay.Server.Sdk.Ecs;
using ReadyM.Relay.Server.Sdk.Rpc;

namespace ExampleMod.Server;

// Server side of the server-RPC contracts. Implement the [ClientToServer] handlers; the
// [ServerToClient] Send methods are generated. Each handler gets an RpcContext with the sender.
// Register in Mod.Init.
[ServerRpcFor(typeof(RpcContracts))]
public partial class ExampleServerRpc(EcsApi ecsApi, ILogger logger) : ServerRpcHandlersBase
{
    // Credit the sender's wallet and reply with the amount added.
    partial void OnAddWalletBalance(RpcContext context)
    {
        const int reward = 100;

        ecsApi.Query<PlayerScopeComponent, WalletComponent>((ref scope, ref wallet) =>
        {
            if (scope.PlayerId != context.Sender) return;

            wallet.Balance += reward;
            logger.LogInformation("Added {Reward} gold for player {Player}. Balance: {Balance}",
                reward, context.Sender, wallet.Balance);

            SendAddWalletBalance(context.Sender, reward);
        });
    }

    // Reply with the sender's current balance.
    partial void OnGetWalletBalance(RpcContext context)
    {
        ecsApi.Query<PlayerScopeComponent, WalletComponent>((ref scope, ref wallet) =>
        {
            if (scope.PlayerId != context.Sender) return;

            SendGetWalletBalance(context.Sender, wallet.Balance);
        });
    }
}
