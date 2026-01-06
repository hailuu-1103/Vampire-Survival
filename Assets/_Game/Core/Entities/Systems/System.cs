#nullable enable

namespace Core.Entities
{
    using System.Collections.Generic;

    public interface IUpdateable : IPauseable
    {
        public void Tick(float deltaTime);
    }

    public interface IPauseable
    {
        public void Pause();
        public void Resume();
    }

    public abstract class System<TTarget> : Component, IUpdateable
        where TTarget : IEntity
    {
        protected readonly HashSet<TTarget> cache  = new();
        private readonly   List<TTarget>    buffer = new();

        private bool isPaused;

        protected sealed override void OnSpawn()
        {
            this.Manager.Spawned  += this.OnEntitySpawned;
            this.Manager.Recycled += this.OnEntityRecycled;

            foreach (var entity in this.Manager.Query<TTarget>()) this.cache.Add(entity);

            this.OnSystemSpawn();
        }

        protected virtual void OnSystemSpawn() { }

        protected virtual void OnEntitySpawned(IEntity entity, IReadOnlyList<IComponent> components)
        {
            if (entity is TTarget target)
            {
                this.cache.Add(target);
            }

            foreach (var component in components)
            {
                if (component is TTarget componentTarget)
                {
                    this.cache.Add(componentTarget);
                }
            }
        }

        protected virtual void OnEntityRecycled(IEntity entity, IReadOnlyList<IComponent> components)
        {
            if (entity is TTarget target)
            {
                this.cache.Remove(target);
            }

            foreach (var component in components)
            {
                if (component is TTarget componentTarget)
                {
                    this.cache.Remove(componentTarget);
                }
            }
        }

        protected abstract bool Filter(TTarget entity);
        protected abstract void Apply(TTarget  entity);

        protected void ProcessEntity(TTarget entity)
        {
            if (!this.Filter(entity)) return;
            this.Apply(entity);
        }

        void IUpdateable.Tick(float deltaTime)
        {
            if (this.isPaused) return;

            if (this.buffer.Capacity < this.cache.Count) this.buffer.Capacity = this.cache.Count;

            this.buffer.Clear();
            this.buffer.AddRange(this.cache);

            foreach (var entity in this.buffer)
            {
                if (!this.cache.Contains(entity)) continue;
                this.ProcessEntity(entity);
            }
        }

        void IPauseable.Pause()  => this.isPaused = true;
        void IPauseable.Resume() => this.isPaused = false;

        protected sealed override void OnRecycle()
        {
            this.OnSystemRecycle();

            this.Manager.Spawned  -= this.OnEntitySpawned;
            this.Manager.Recycled -= this.OnEntityRecycled;

            this.cache.Clear();
            this.buffer.Clear();
        }

        protected virtual void OnSystemRecycle() { }
    }
}