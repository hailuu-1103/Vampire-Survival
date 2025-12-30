#nullable enable

namespace VampireSurvival.Core.Events
{
    using VampireSurvival.Core.Abstractions;

    public sealed record EnemyDiedEvent(IEnemy Enemy);
}