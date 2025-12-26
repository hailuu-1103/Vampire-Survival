#nullable enable

namespace Core.Observer.DI
{
    using VContainer;

    public static class ObserverInstaller
    {
        public static void RegisterObserver(this IContainerBuilder builder)
        {
            builder.Register<EventBus>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}