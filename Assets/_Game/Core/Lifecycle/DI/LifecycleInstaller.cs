#nullable enable
namespace Core.Lifecycle.DI
{
    using VContainer;

    public static class LifecycleInstaller
    {
        public static void RegisterLifecycle(this IContainerBuilder builder)
        {
            builder.Register<LifecycleManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
