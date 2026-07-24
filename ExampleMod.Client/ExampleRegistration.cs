using ExampleMod.Common;
using OblivionMp.Sdk;
using ReadyM.Api.ECS.Registry;
using ReadyM.Api.ECS.Worlds;

namespace ExampleMod.Client;

// Attaches networked components to archetypes while the ECS schema is built. Archetype membership
// must match the server mod. Registered as an IArchetypeRegistration in Mod.RegisterServices.
public class ExampleRegistration : IArchetypeRegistration
{
    public void Register(IArchetypeRegistry registry)
    {
        registry.ModifyArchetype(SDK.Archetypes.GlobalPlayerArchetype, b => b.Add<WalletComponent>());
    }
}
