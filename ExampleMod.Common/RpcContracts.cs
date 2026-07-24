using ReadyM.Api.Multiplayer;

namespace ExampleMod.Common;

// Server-RPC contracts shared by both mods. [ClientToServer] generates a Send on the client and a
// handler on the server; [ServerToClient] the reverse. Same name, opposite directions = request/response.
[ServerRpcContracts]
public static partial class RpcContracts
{
    // Client asks the server to add gold; the server replies with the amount that was added.
    [ClientToServer] public static partial void AddWalletBalance();
    [ServerToClient] public static partial void AddWalletBalance(int amount);

    // Client asks the server for the current balance; the server replies with it.
    [ClientToServer] public static partial void GetWalletBalance();
    [ServerToClient] public static partial void GetWalletBalance(int amount);
}
