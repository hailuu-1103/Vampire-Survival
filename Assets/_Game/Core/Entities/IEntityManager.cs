#nullable enable
namespace Core.Entities
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public interface IEntityManager
    {
        public event Action<IEntity, IReadOnlyList<IComponent>> Instantiated;

        public event Action<IEntity, IReadOnlyList<IComponent>> Spawned;

        public event Action<IEntity, IReadOnlyList<IComponent>> Recycled;

        public event Action<IEntity, IReadOnlyList<IComponent>> CleanedUp;

        public void Load(IEntity prefab, int count = 1);

        public TEntity Spawn<TEntity>(TEntity prefab, Vector3 position = default, Quaternion rotation = default, Transform? parent = null, bool spawnInWorldSpace = true) where TEntity : IEntityWithoutParams;

        public TEntity Spawn<TEntity>(TEntity prefab, object @params, Vector3 position = default, Quaternion rotation = default, Transform? parent = null, bool spawnInWorldSpace = true) where TEntity : IEntityWithParams;

        public TEntity Spawn<TEntity>(Vector3 position = default, Quaternion rotation = default, Transform? parent   = null,    bool       spawnInWorldSpace = true) where TEntity : Object, IEntityWithoutParams;
        public TEntity Spawn<TEntity>(object  @params,            Vector3    position = default, Quaternion rotation = default, Transform? parent            = null, bool spawnInWorldSpace = true) where TEntity : IEntityWithParams;
        public void    Recycle(IEntity        instance);

        public void RecycleAll(IEntity prefab);

        public void Cleanup(IEntity prefab, int retainCount = 1);

        public void Unload(IEntity prefab);

        public IEnumerable<T> Query<T>();

        public void RegisterComponent(IEntity entity, IComponent component);

        public void UnregisterComponent(IComponent component);
    }
}