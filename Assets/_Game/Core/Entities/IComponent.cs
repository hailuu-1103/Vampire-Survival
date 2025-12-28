#nullable enable
namespace Core.Entities
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using Core.DI;

    public interface IComponent : IComponentLifecycle
    {
        public IDependencyContainer Container { set; }

        public IEntityManager Manager { get; set; }

        public IEntity Entity { get; set; }

        // ReSharper disable once InconsistentNaming
        public GameObject gameObject { get; }

        // ReSharper disable once InconsistentNaming
        public Transform transform { get; }
    }
}