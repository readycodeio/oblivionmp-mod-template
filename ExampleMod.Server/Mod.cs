using ExampleMod.Common;
using Microsoft.Extensions.Logging;
using OblivionMp.Sdk.Serverside;
using ReadyM.Api.ECS.Worlds;
using ReadyM.Relay.Server.Sdk;
using ReadyM.Relay.Server.Sdk.Ecs.Components;
using ReadyM.Relay.Server.Sdk.Ecs.Systems;

namespace ExampleMod.Server;

// The server mod's entry point. The plugin host instantiates the single ServerModBase-derived class.
public class Mod : ServerModBase
{
    // Runs first, before the ECS schema is finalized. The only place to register components.
    protected override void RegisterComponents(IComponentRegistry registry)
    {
        registry.RegisterComponent<WalletComponent>();
    }

    // Runs after RegisterComponents. Register RPC handlers and systems, attach components.
    protected override void Init()
    {
        Services.RegisterSingleton<ExampleServerRpc>();
        Services.RegisterSingleton<ModSystemBase, PassiveIncomeSystem>();

        // Attach WalletComponent to the global player archetype. Must match the client mod.
        var registry = Services.Resolve<IArchetypeRegistry>();
        var archetypes = Services.Resolve<OblivionArchetypes>();
        registry.ModifyArchetype(archetypes.GlobalPlayerArchetype, a => a.Add<WalletComponent>());

        Services.Resolve<ILogger>().LogInformation("Example server mod initialized");
    }
}
