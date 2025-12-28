#nullable enable

namespace Game
{
    using System;
    using Core.DI.DI;
    using Core.Entities;
    using Core.Entities.DI;
    using Core.Observer.DI;
    using UnityEngine;
    using VampireSurvival.Core.DI;
    using VampireSurvival.Core.Entities;
    using VContainer;
    using VContainer.Unity;

    public class TestScope : LifetimeScope
    {
        [SerializeField] private Player            player        = null!;
        [SerializeField] private GameSystemsRunner systemsRunner = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterDI();
            builder.RegisterObserver();
            builder.RegisterEntityManager();
            builder.RegisterVampireSurvival();
            builder.RegisterBuildCallback(container =>
            {
                var objectPoolManager = container.Resolve<IEntityManager>();
                objectPoolManager.Spawn(this.player);
                objectPoolManager.Spawn(this.systemsRunner);
            });
        }
    }
}