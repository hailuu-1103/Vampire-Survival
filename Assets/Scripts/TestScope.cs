#nullable enable

namespace Game
{
    using Core.DI;
    using Core.GameFlow;
    using UnityEngine;
    using VampireSurvival.Core;
    using VampireSurvival.Core.Entities;
    using VampireSurvival.Core.UI;
    using VContainer;
    using VContainer.Unity;

    public sealed class TestScope : SceneScope
    {
        [SerializeField] private GameCanvas        gameCanvas    = null!;
        [SerializeField] private GameSystemsRunner systemsRunner = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponentInHierarchy<GameCamera>();
            builder.RegisterBuildCallback(container =>
            {
                var gameplayService = container.Resolve<IGameplayService>();
                gameplayService.Load();
                gameplayService.Play();
            });
        }
    }
}