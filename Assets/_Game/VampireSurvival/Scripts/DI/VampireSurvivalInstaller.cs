#nullable enable

namespace VampireSurvival.Core.DI
{
    using UnityEngine;
    using VampireSurvival.Core.Services;
    using VampireSurvival.Core.Stats;
    using VampireSurvival.Core.Systems;
    using VContainer;

    public static class VampireSurvivalInstaller
    {
        public static void RegisterVampireSurvival(this IContainerBuilder builder)
        {
            builder.Register(_ => Resources.Load<EnemyConfig>(nameof(EnemyConfig)), Lifetime.Singleton);
            builder.Register<VampireSurvivalService>(Lifetime.Singleton).AsSelf();

            builder.Register<PlayerMovementSystem>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<EnemySpawnSystem>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<EnemyChaseSystem>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<EnemyAttackSystem>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}