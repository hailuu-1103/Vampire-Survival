#nullable enable

namespace Game
{
    using Core.DI;
    using Core.GameFlow;
    using Game.UI;
    using VampireSurvival.Core;
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
                gameplayService.Play();
            });
        }
    }
}