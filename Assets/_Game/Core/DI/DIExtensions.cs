#nullable enable

namespace Core.DI
{
    using UnityEngine;
    using VContainer;

    public static class DIExtensions
    {
        private static SceneScope? CurrentSceneContext;

        public static IDependencyContainer GetCurrentContainer()
        {
            if (CurrentSceneContext == null) CurrentSceneContext = Object.FindAnyObjectByType<SceneScope>();
            return CurrentSceneContext.Container.Resolve<IDependencyContainer>();
        }

        public static IDependencyContainer GetCurrentContainer(this object _)
        {
            return GetCurrentContainer();
        }

    }
}