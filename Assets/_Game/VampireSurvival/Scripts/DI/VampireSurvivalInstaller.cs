#nullable enable

using Core.Utils;
using IReactiveSystem = Core.Entities.IReactiveSystem;

namespace VampireSurvival.DI
{
    using VampireSurvival.Configs;
    using VampireSurvival.Progression.Services;
    using VampireSurvival.Services;
    using VContainer;

    public static class VampireSurvivalInstaller
    {
        public static void RegisterVampireSurvival(this IContainerBuilder builder)
        {
            builder.RegisterResource<EnemyConfig>(nameof(EnemyConfig), Lifetime.Singleton).AsSelf();
            builder.RegisterResource<PlayerConfig>(nameof(PlayerConfig), Lifetime.Singleton).AsSelf();
            builder.RegisterResource<ProjectileConfig>(nameof(ProjectileConfig), Lifetime.Singleton).AsSelf();
            builder.Register<VampireSurvivalService>(Lifetime.Singleton).AsSelf();
            builder.Register<PlayerProgressionService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            typeof(IReactiveSystem).GetDerivedTypes().ForEach(type => builder.Register(type, Lifetime.Singleton).AsImplementedInterfaces());
        }
    }
}