#nullable enable

namespace Game
{
    using Core.DI.DI;
    using Core.Entities.DI;
    using Core.FTUE.DI;
    using Core.GameFlow.DI;
    using Core.Lifecycle.DI;
    using Core.Observer.DI;
    using Core.ScreenFlow.DI;
    using Game.FTUE;
    using Game.Services;
    using UnityEngine;
    using VampireSurvival.Core.DI;
    using VContainer;
    using VContainer.Unity;

    public class GameLifeTimeScope : LifetimeScope
    {
        [SerializeField] private FTUEConfig ftueConfig = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterDI();
            builder.RegisterObserver();
            builder.RegisterEntityManager();
            builder.RegisterGameFlow();
            builder.RegisterScreenFlow();
            builder.RegisterLifecycle();
            builder.RegisterFTUE();
            builder.RegisterVampireSurvival();
            builder.RegisterInstance(this.ftueConfig).AsSelf();
            builder.Register<KillCountTracker>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<PlayerLevelUpBonusService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}