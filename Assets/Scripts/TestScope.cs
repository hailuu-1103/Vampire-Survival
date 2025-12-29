#nullable enable

namespace Game
{
    using Core.DI;
    using Core.Entities;
    using Core.GameFlow;
    using Game.UI;
    using UnityEngine;
    using VampireSurvival.Core;
    using VampireSurvival.Core.Entities;
    using VContainer;
    using VContainer.Unity;

    public sealed class TestScope : SceneScope
    {

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<GameCanvas>();
            builder.RegisterComponentInHierarchy<GameCamera>();
            builder.RegisterBuildCallback(container =>
            {
                var gameplayService = container.Resolve<IGameplayService>();
                gameplayService.Load();
                gameplayService.Resume();
            });
        }
    }
}