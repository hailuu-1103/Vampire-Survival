#nullable enable

namespace VampireSurvival.Events
{
    using VampireSurvival.Abstractions;

    public sealed record EnemyDiedEvent(IEnemy Enemy);
}
