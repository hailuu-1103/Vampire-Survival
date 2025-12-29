#nullable enable

namespace Core.GameFlow.DI
{
    using Core.Utils;
    using VContainer;

    public static class GameFlowInstaller
    {
        public static void RegisterGameFlow(this IContainerBuilder builder)
        {
            builder.Register(typeof(IGameplayService).GetSingleDerivedType(), Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}