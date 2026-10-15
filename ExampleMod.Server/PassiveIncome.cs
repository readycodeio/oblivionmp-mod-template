using ExampleMod.Common;
using ReadyM.SDK.Archetypes.Core;
using ReadyM.SDK.Attributes;
using ReadyM.SDK.Server.Entities;

namespace ExampleMod.Server;

// A service with an Update, which the server calls every tick. Time says how long the last tick took.
// Grants every player a little gold on an interval.
[Service]
public sealed partial class PassiveIncome(IEntities entities)
{
    private const float IntervalSeconds = 10f;
    private const int Income = 10;

    private float _elapsed;

    private void Update()
    {
        _elapsed += Time.DeltaTime;
        if (_elapsed < IntervalSeconds)
            return;

        _elapsed -= IntervalSeconds;

        // Inside a loop over a query, write with SetBalance rather than assigning Balance.
        foreach (var player in entities.Query<Player>())
            player.SetBalance(player.Balance + Income);
    }
}
