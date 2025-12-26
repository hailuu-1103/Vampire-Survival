#nullable enable
namespace Core.Entities
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class EntityBase : MonoBehaviour, IEntity
    {
        public GameObject GameObject => this.gameObject;

        private readonly List<IComponent> components = new();
        private          bool             componentsBuilt;

        public IReadOnlyList<IComponent> Components => this.components;

        protected virtual void BuildComponents()
        {
        }

        protected virtual void Awake()
        {
            if (this.componentsBuilt)
                return;

            this.componentsBuilt = true;
            this.BuildComponents();
        }

        protected void AddComponent(IComponent component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            var type = component.GetType();

            if (this.components.Exists(c => c.GetType() == type))
            {
                Debug.LogError(
                    $"Duplicate component {type.Name} on entity {this.name}",
                    this
                );
                return;
            }

            this.components.Add(component);
        }

        public T Get<T>() where T : class, IComponent
        {
            if (this.TryGet<T>(out var component))
                return component;

            throw new InvalidOperationException(
                $"Component {typeof(T).Name} not found on {this.name}"
            );
        }

        public bool TryGet<T>(out T? component) where T : class, IComponent
        {
            component = this.components.Find(c => c is T) as T;
            return component != null;
        }

        public bool Has<T>() where T : class, IComponent
            => this.components.Exists(c => c is T);
    }
}