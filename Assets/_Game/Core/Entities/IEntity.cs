#nullable enable
namespace Core.Entities
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IEntity
    {
        public IReadOnlyList<IComponent> Components { get; }
        public GameObject                GameObject { get; }

        public T    Get<T>() where T : class, IComponent;
        public bool TryGet<T>(out T? component) where T : class, IComponent;
        public bool Has<T>() where T : class, IComponent;
    }
    public interface IEntityWithParams<in TParams>
    {
        void Apply(TParams @params);
    }
}