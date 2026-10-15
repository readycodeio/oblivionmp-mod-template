using ReadyM.SDK.Archetypes.Core;
using ReadyM.SDK.Attributes;

namespace ExampleMod.Common;

// A mixin adds fields to entities. [Extends(typeof(Player))] gives every player one, so both halves
// read it as player.Balance. [Replicated] sends it to the clients, and ServerAuthoritative means
// only the server writes it: on the client, Balance is read-only.
[ArchetypeMixin]
[Extends(typeof(Player))]
[Replicated]
[Propagates(Propagation.ServerAuthoritative)]
public readonly partial struct Wallet
{
    public partial int Balance { get; set; }
}
