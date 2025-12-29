#nullable enable

namespace Game
{
    using Core.DI.DI;
    using Core.Entities.DI;
    using Core.GameFlow.DI;
    using Core.Observer.DI;
    using Core.ScreenFlow.DI;
    using VampireSurvival.Core.DI;
    using VContainer;
    using VContainer.Unity;

    public class GameLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterDI();
            builder.RegisterObserver();
            builder.RegisterEntityManager();
            builder.RegisterGameFlow();
            builder.RegisterScreenFlow();
            builder.RegisterVampireSurvival();
        }
    }
}