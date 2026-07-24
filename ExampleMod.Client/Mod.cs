using ExampleMod.Common;
using OblivionMp.Sdk;
using ReadyM.Api.DI;
using ReadyM.Api.ECS.Registry;
using ReadyM.Sdk.Common;
using ReadyM.Sdk.Common.Api;
using ReadyM.Sdk.Common.Input;

namespace ExampleMod.Client;

// The mod's entry point. The modloader instantiates the single class deriving from ModBase.
public class Mod : ModBase
{
    public override string Name => "Example Mod"; // TODO: CHANGE ME

    private ExampleServerRpc _serverRpc = null!;

    // Runs once when the mod loads, before Start. Register RPC handlers, services and components.
    protected override void RegisterServices(IDependencyContainer services)
    {
        // RPC handler classes must be registered for their generated handlers to be wired up.
        services.RegisterSingleton<ExampleServerRpc>();
        services.RegisterSingleton<ExampleClientRpc>();

        // Register the shared component, then attach it to an archetype (see ExampleRegistration).
        services.Resolve<IComponentApi>().RegisterComponent<WalletComponent>();
        services.RegisterSingleton<IArchetypeRegistration, ExampleRegistration>();

        _serverRpc = services.Resolve<ExampleServerRpc>();
    }

    // Runs once after all mods have loaded. Bind input and do one-off setup here.
    public override void Start()
    {
        SDK.Input.RegisterKeyBind(ModifierKeys.Alt, Key.N, () => _serverRpc.SendAddWalletBalance());
        SDK.Input.RegisterKeyBind(ModifierKeys.Alt, Key.B, () => _serverRpc.SendGetWalletBalance());
    }
}
