#nullable enable

using IEntity = Core.Entities.IEntity;
using Core.Utils;
using Entities_Component = Core.Entities.Component;
using IComponent = Core.Entities.IComponent;

namespace VampireSurvival.Core.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using VampireSurvival.Core.Abstractions;

    public abstract class System<TTarget> : Entities_Component, IUpdateable where TTarget : IEntity
    {
        protected readonly List<TTarget> cache = new();
        private bool isPaused;

        protected sealed override void OnSpawn()
        {
            this.Manager.Spawned  += this.OnEntitySpawned;
            this.Manager.Recycled += this.OnEntityRecycled;

            foreach (var entity in this.Manager.Query<TTarget>())
            {
                this.cache.Add(entity);
            }

            this.OnSystemSpawn();
        }

        protected virtual void OnSystemSpawn() { }

        protected virtual void OnEntitySpawned(IEntity entity, IReadOnlyList<IComponent> components)
        {
            if (entity is not TTarget target) return;
            this.cache.Add(target);
        }

        protected virtual void OnEntityRecycled(IEntity entity, IReadOnlyList<IComponent> components)
        {
            if (entity is not TTarget target) return;
            this.cache.Remove(target);
        }

        protected abstract bool Filter(TTarget entity);

        protected abstract void Apply(TTarget entity);

        void IUpdateable.Tick(float deltaTime)
        {
            if (this.isPaused) return;
            this.cache.Where(this.Filter).ForEach(this.Apply);
        }

        void IPauseable.Pause()  => this.isPaused = true;
        void IPauseable.Resume() => this.isPaused = false;

        protected sealed override void OnRecycle()
        {
            this.OnSystemRecycle();

            this.Manager.Spawned  -= this.OnEntitySpawned;
            this.Manager.Recycled -= this.OnEntityRecycled;

            this.cache.Clear();
        }

        protected virtual void OnSystemRecycle() { }
    }
}