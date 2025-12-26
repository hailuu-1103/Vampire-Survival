namespace Core.DI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class VContainerWrapper : IDependencyContainer
    {
        private readonly IObjectResolver container;

        public VContainerWrapper(IObjectResolver container)
        {
            this.container = container;
        }

        object IDependencyContainer.Resolve(Type type)
        {
            return this.container.Resolve(type);
        }

        T IDependencyContainer.Resolve<T>()
        {
            return this.container.Resolve<T>();
        }

        object[] IDependencyContainer.ResolveAll(Type type)
        {
            return ((IEnumerable)this.container.Resolve(typeof(IEnumerable<>).MakeGenericType(type))).Cast<object>().ToArray();
        }

        T[] IDependencyContainer.ResolveAll<T>()
        {
            return this.container.Resolve<IEnumerable<T>>().ToArray();
        }

        void IDependencyContainer.Inject(object instance)
        {
            this.container.Inject(instance);
        }

        void IDependencyContainer.InjectGameObject(GameObject instance)
        {
            this.container.InjectGameObject(instance);
        }

        GameObject IDependencyContainer.InstantiatePrefab(GameObject prefab)
        {
            return this.container.Instantiate(prefab);
        }
    }
}