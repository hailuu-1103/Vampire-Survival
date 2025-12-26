#nullable enable

namespace Game
{
    using Core.DI.DI;
    using Core.Entities;
    using Core.Entities.DI;
    using Core.Observer.DI;
    using UnityEngine;
    using VampireSurvival.Core;
    using VampireSurvival.Core.DI;
    using VContainer;
    using VContainer.Unity;

    public class TestScope : LifetimeScope
    {
        [SerializeField] private Player player = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterDI();
            builder.RegisterObserver();
            builder.RegisterEntityManager();
            builder.RegisterVampireSurvival();

            builder.RegisterBuildCallback(container =>
            {
                var entityManager = container.Resolve<IEntityManager>();
                var player        = entityManager.Spawn(this.player);

                Debug.Log("Player spawned with HP = " +
                    player.Get<HealthComponent>().Current);
            });
        }
    }
}