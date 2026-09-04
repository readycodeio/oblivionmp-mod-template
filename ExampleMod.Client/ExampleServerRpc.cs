using ExampleMod.Common;
using Microsoft.Extensions.Logging;
using OblivionMp.Sdk;
using ReadyM.Api.Multiplayer;
using ReadyM.Api.Multiplayer.RPC;

namespace ExampleMod.Client;

// Client side of the server-RPC contracts. Implement the [ServerToClient] handlers; the
// [ClientToServer] Send methods are generated. Register in Mod.RegisterServices.
[ServerRpcFor(typeof(RpcContracts))]
public partial class ExampleServerRpc(ILogger logger) : ServerRpcClient
{
    partial void OnAddWalletBalance(int amount)
    {
        logger.LogInformation("Server added {Amount} gold to your wallet.", amount);

        if (SDK.Sync.LocalPlayer is { } player)
            logger.LogInformation("New balance: {Balance}", player.GetGlobal<WalletComponent>().Balance);
    }

    partial void OnGetWalletBalance(int amount)
    {
        logger.LogInformation("Your server-side balance is {Amount} gold.", amount);
    }
}
