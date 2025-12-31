#nullable enable
namespace Core.FTUE.DI
{
    using VContainer;

    public static class FTUEInstaller
    {
        public static void RegisterFTUE(this IContainerBuilder builder)
        {
            builder.Register<FTUEService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
