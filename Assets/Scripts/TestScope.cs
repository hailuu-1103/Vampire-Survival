#nullable enable

namespace Game
{
    using Core.DI;
    using Core.GameFlow;
    using Game.FTUE;
    using Game.UI;
    using VampireSurvival.Components;
    using VContainer;
    using VContainer.Unity;

    public sealed class TestScope : SceneScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<FTUEHandler>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterComponentInHierarchy<GameCanvas>();
            builder.RegisterComponentInHierarchy<GameCamera>();
            builder.RegisterBuildCallback(container =>
            {
                var gameplayService = container.Resolve<IGameplayService>();
                gameplayService.Load();
            });
        }
    }
}