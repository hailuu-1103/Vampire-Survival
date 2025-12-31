#nullable enable

using Core.Utils;

namespace VampireSurvival.Core.DI
{
    using VampireSurvival.Core.Services;
    using VampireSurvival.Core.Stats;
    using VampireSurvival.Core.Systems;
    using VContainer;

    public static class VampireSurvivalInstaller
    {
        public static void RegisterVampireSurvival(this IContainerBuilder builder)
        {
            builder.RegisterResource<EnemyConfig>(nameof(EnemyConfig), Lifetime.Singleton).AsSelf();
            builder.RegisterResource<PlayerConfig>(nameof(PlayerConfig), Lifetime.Singleton).AsSelf();
            builder.Register<VampireSurvivalService>(Lifetime.Singleton).AsSelf();
            builder.Register<PlayerProgressionService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            typeof(IReactiveSystem).GetDerivedTypes().ForEach(type => builder.Register(type, Lifetime.Singleton).AsImplementedInterfaces());
        }
    }
}