#nullable enable

namespace VampireSurvival.Core.DI
{
    using VampireSurvival.Core.Enemies;
    using VContainer;
    using VampireSurvival.Core.Systems;

    public static class VampireSurvivalInstaller
    {
        public static void RegisterVampireSurvival(this IContainerBuilder builder)
        {
            builder.Register<PlayerMovementSystem>(Lifetime.Singleton);
            builder.Register<EnemyAttackSystem>(Lifetime.Singleton);
            builder.Register<EnemyChaseSystem>(Lifetime.Singleton);
            builder.Register<EnemySpawnSystem>(Lifetime.Singleton);
        }
    }
}