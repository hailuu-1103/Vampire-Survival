#nullable enable

namespace Core.Entities.DI
{
    using Core.Pooling.DI;
    using VContainer;

    public static class EntityInstaller
    {
        public static void RegisterEntityManager(this IContainerBuilder builder)
        {
            builder.RegisterPooling();
            builder.Register<EntityManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}