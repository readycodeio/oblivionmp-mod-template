using ExampleMod.Common;
using ReadyM.Relay.Server.Sdk.Ecs;
using ReadyM.Relay.Server.Sdk.Ecs.Systems;

namespace ExampleMod.Server;

// A gameplay system running on the server tick (registered as a ModSystemBase in Mod.Init).
// Grants passive income to every wallet on an interval.
public class PassiveIncomeSystem(EcsApi ecsApi) : ModSystemBase
{
    private const float IntervalSeconds = 10f;
    private const int IncomeAmount = 10;

    private float _elapsed;

    protected override void OnUpdate(UpdateTick tick)
    {
        _elapsed += tick.deltaTime;
        if (_elapsed < IntervalSeconds) return;

        _elapsed -= IntervalSeconds;
        ecsApi.Query<WalletComponent>((ref wallet) => wallet.Balance += IncomeAmount);
    }
}
