namespace Core.Pooling.DI
{
    using VContainer;

    public static class ObjectPoolInstaller
    {
        public static void RegisterPooling(this IContainerBuilder builder)
        {
            builder.Register<ObjectPoolManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}