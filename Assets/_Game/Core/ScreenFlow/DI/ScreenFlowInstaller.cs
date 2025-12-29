#nullable enable

namespace Core.ScreenFlow.DI
{
    using VContainer;

    public static class ScreenFlowInstaller
    {
        public static void RegisterScreenFlow(this IContainerBuilder builder)
        {
            builder.Register<ScreenManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}