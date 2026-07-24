using System.Runtime.InteropServices;
using ReadyM.Api.Mapping.Tags;
using ReadyM.Api.Multiplayer.Generators;

namespace ExampleMod.Common;

// Networked component. [DeriveINetworkedComponent] generates serialization and a public Balance
// property from _balance. IOwnershipBased: only the owner (or the server) may write it.
[DeriveINetworkedComponent]
[StructLayout(LayoutKind.Auto)]
public partial struct WalletComponent : IOwnershipBased
{
    private int _balance;
}
