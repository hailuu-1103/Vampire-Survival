#nullable enable

namespace Core.Utils
{
    using System;
    using UnityEngine;
    using VContainer;
    using Object = UnityEngine.Object;

    public static class VContainerExtensions
    {
        public static RegistrationBuilder RegisterResource<T>(this IContainerBuilder builder, string path, Lifetime lifetime) where T : Object
        {
            return builder.Register(_ => Resources.Load<T>(path) ?? throw new ArgumentOutOfRangeException(nameof(path), path, $"Failed to load {path}"), lifetime);
        }
    }
}