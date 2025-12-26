#nullable enable

namespace Core.DI.DI
{
    using VContainer;

    public static class DIInstaller
    {
        public static void RegisterDI(this IContainerBuilder builder)
        {
            builder.Register<VContainerWrapper>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}