#nullable enable

namespace Game
{
    using Core.DI.DI;
    using Core.Entities;
    using Core.Entities.DI;
    using Core.Observer.DI;
    using UnityEngine;
    using VampireSurvival.Core.DI;
    using VampireSurvival.Core.Entities;
    using VampireSurvival.Core.UI;
    using VContainer;
    using VContainer.Unity;

    public class GameLifeTimeScope : LifetimeScope
    {
        [SerializeField] private Player            player        = null!;
        [SerializeField] private GameCanvas        gameCanvas    = null!;
        [SerializeField] private GameSystemsRunner systemsRunner = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterDI();
            builder.RegisterObserver();
            builder.RegisterEntityManager();
            builder.RegisterVampireSurvival();
            builder.RegisterBuildCallback(container =>
            {
                var entityManager     = container.Resolve<IEntityManager>();
                entityManager.Spawn(this.player);
                entityManager.Spawn(this.systemsRunner);
                entityManager.Spawn(this.gameCanvas);
            });
        }
    }
}