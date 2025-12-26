#nullable enable
namespace Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Pooling;
    using UnityEngine;

    public sealed class EntityManager : IEntityManager
    {
        private readonly IObjectPoolManager pool;

        private readonly Dictionary<GameObject, IEntity> objToEntity = new();
        private readonly HashSet<IEntity> aliveEntities = new();
        private readonly Dictionary<Type, HashSet<IComponent>> componentsByType = new();
        private readonly Stack<object?> spawnParams = new();

        public event Action<IEntity, IReadOnlyList<IComponent>>? Instantiated;
        public event Action<IEntity, IReadOnlyList<IComponent>>? Spawned;
        public event Action<IEntity, IReadOnlyList<IComponent>>? Recycled;
        public event Action<IEntity, IReadOnlyList<IComponent>>? CleanedUp;

        public EntityManager(IObjectPoolManager pool)
        {
            this.pool = pool;

            pool.Instantiated += this.OnInstantiated;
            pool.Spawned      += this.OnSpawned;
            pool.Recycled     += this.OnRecycled;
            pool.CleanedUp    += this.OnCleanedUp;
        }


        private void OnInstantiated(GameObject go)
        {
            if (!go.TryGetComponent<IEntity>(out var entity))
                return;

            this.objToEntity[go] = entity;
            this.Instantiated?.Invoke(entity, entity.Components);
        }

        private void OnSpawned(GameObject go)
        {
            if (!this.objToEntity.TryGetValue(go, out var entity))
                return;

            if (this.spawnParams.Count > 0)
            {
                var p = this.spawnParams.Peek();
                if (p != null)
                {
                    var iface = entity
                        .GetType()
                        .GetInterfaces()
                        .FirstOrDefault(i =>
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(IEntityWithParams<>));

                    iface?.GetMethod("Apply")?.Invoke(entity, new[] { p });
                }
            }

            this.aliveEntities.Add(entity);

            foreach (var component in entity.Components)
            {
                var type = component.GetType();
                if (!this.componentsByType.TryGetValue(type, out var set))
                {
                    set = new HashSet<IComponent>();
                    this.componentsByType[type] = set;
                }
                set.Add(component);
            }

            this.Spawned?.Invoke(entity, entity.Components);
        }

        private void OnRecycled(GameObject go)
        {
            if (!this.objToEntity.TryGetValue(go, out var entity))
                return;

            this.aliveEntities.Remove(entity);

            // Remove component index
            foreach (var component in entity.Components)
            {
                var type = component.GetType();
                if (this.componentsByType.TryGetValue(type, out var set))
                {
                    set.Remove(component);
                }
            }

            this.Recycled?.Invoke(entity, entity.Components);
        }

        private void OnCleanedUp(GameObject go)
        {
            if (!this.objToEntity.Remove(go, out var entity))
                return;

            this.CleanedUp?.Invoke(entity, entity.Components);
        }

        // ---------------- API ----------------

        public void Load(IEntity prefab, int count = 1)
        {
            if (prefab is not Component c)
                throw new InvalidOperationException("Entity prefab must be a Unity Component");

            this.pool.Load(c.gameObject, count);
        }

        public TEntity Spawn<TEntity>(
            TEntity prefab,
            Vector3 position = default,
            Quaternion rotation = default,
            Transform? parent = null,
            bool spawnInWorldSpace = true
        )
            where TEntity : Component, IEntity
        {
            this.spawnParams.Push(null);

            var go = this.pool.Spawn(
                prefab.gameObject,
                position,
                rotation,
                parent,
                spawnInWorldSpace
            );

            this.spawnParams.Pop();
            return (TEntity)go.GetComponent(typeof(TEntity));
        }

        public TEntity Spawn<TEntity, TParams>(
            TEntity prefab,
            TParams @params,
            Vector3 position = default,
            Quaternion rotation = default,
            Transform? parent = null,
            bool spawnInWorldSpace = true
        )
            where TEntity : Component, IEntity, IEntityWithParams<TParams>
        {
            this.spawnParams.Push(@params);

            var go = this.pool.Spawn(
                prefab.gameObject,
                position,
                rotation,
                parent,
                spawnInWorldSpace
            );

            this.spawnParams.Pop();
            return (TEntity)go.GetComponent(typeof(TEntity));
        }

        public void Recycle(IEntity instance)
        {
            if (instance is not Component c)
                return;

            this.pool.Recycle(c.gameObject);
        }

        public void RecycleAll(IEntity prefab)
        {
            if (prefab is not Component c)
                return;

            this.pool.RecycleAll(c.gameObject);
        }

        public void Cleanup(IEntity prefab, int retainCount = 1)
        {
            if (prefab is not Component c)
                return;

            this.pool.Cleanup(c.gameObject, retainCount);
        }

        public void Unload(IEntity prefab)
        {
            if (prefab is not Component c)
                return;

            this.pool.Unload(c.gameObject);
        }


        public IEnumerable<T> Query<T>() where T : IComponent
        {
            if (!this.componentsByType.TryGetValue(typeof(T), out var set))
                yield break;

            // snapshot to avoid mutation issues
            foreach (var c in set.ToArray())
            {
                yield return (T)c;
            }
        }

        public IEnumerable<IEntity> QueryEntities()
            => this.aliveEntities;
    }
}