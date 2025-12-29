#nullable enable

using Core.Utils;
namespace VampireSurvival.Core.DI
{
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Services;
    using VampireSurvival.Core.Stats;
    using VContainer;

    public static class VampireSurvivalInstaller
    {
        public static void RegisterVampireSurvival(this IContainerBuilder builder)
        {
            builder.Register(_ => Resources.Load<EnemyConfig>(nameof(EnemyConfig)), Lifetime.Singleton);
            builder.Register<VampireSurvivalService>(Lifetime.Singleton).AsSelf();
            typeof(ISystem).GetDerivedTypes().ForEach(type => builder.Register(type, Lifetime.Singleton).AsImplementedInterfaces().AsSelf());
        }
    }
}