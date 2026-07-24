using Microsoft.Extensions.Logging;
using ReadyM.Api.Multiplayer.Generators;
using ReadyM.Api.Multiplayer.Protocol.Enums;
using ReadyM.Api.Multiplayer.RPC;

namespace ExampleMod.Client;

// A client-relayed RPC: sent by one client, forwarded by the server to other clients, no server
// logic. "On..." handlers are tagged with [RpcEvent]; a matching "Send..." is generated. Register
// in Mod.RegisterServices.
public partial class ExampleClientRpc(ILogger logger) : ClientRpcHandler
{
    [RpcEvent(RelayMode.AreaOfInterestOthers)]
    private void OnPing(string message)
    {
        logger.LogInformation("Ping from another player: {Message}", message);
    }
}
