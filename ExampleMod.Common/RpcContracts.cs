using ReadyM.Api.Multiplayer;
using ReadyM.Api.Multiplayer.Protocol.Enums;
using ReadyM.SDK.Attributes;

namespace ExampleMod.Common;

// Every message the mod sends, shared by both halves. Each one generates a Send method on the half
// that sends it and a handler to implement on the half that receives it. The same name in opposite
// directions makes a request and its response.
[RpcContracts]
public static partial class RpcContracts
{
    // The client asks the server to add gold; the server replies with the amount it added.
    [ClientToServer] public static partial void AddWalletBalance();
    [ServerToClient] public static partial void AddWalletBalance(int amount);

    // The client asks for its balance; the server replies with it.
    [ClientToServer] public static partial void GetWalletBalance();
    [ServerToClient] public static partial void GetWalletBalance(int amount);

    // Sent by one client to the other players nearby. The server only relays it.
    [ClientToClients(RelayMode.AreaOfInterestOthers)] public static partial void Ping(string message);
}
