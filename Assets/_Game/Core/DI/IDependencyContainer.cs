#nullable enable
namespace Core.DI
{
    using System;
    using UnityEngine;

    public interface IDependencyContainer
    {
        public object Resolve(Type type);

        public T Resolve<T>();

        public object[] ResolveAll(Type type);

        public T[] ResolveAll<T>();

        public void Inject(object instance);

        public void InjectGameObject(GameObject instance);

        public GameObject InstantiatePrefab(GameObject prefab);
    }
}