#nullable enable

namespace Game
{
    using Core.DI;
    using Core.Entities;
    using Core.GameFlow;
    using UnityEngine;
    using VampireSurvival.Core;
    using VampireSurvival.Core.Components;
    using VampireSurvival.Core.Entities;
    using VContainer;
    using VContainer.Unity;

    public sealed class TestScope : SceneScope
    {
        [SerializeField] private GameManager gameManager = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponentInHierarchy<GameCamera>();
            builder.RegisterBuildCallback(container =>
            {
                var entityManager = container.Resolve<IEntityManager>();
                entityManager.Spawn(this.gameManager);
                var gameplayService = container.Resolve<IGameplayService>();
                gameplayService.Load();
                gameplayService.Play();
            });
        }
    }
}