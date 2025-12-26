#nullable enable

namespace Core.Entities
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;


    public interface IEntityManager
    {
        event Action<IEntity, IReadOnlyList<IComponent>> Instantiated;

        event Action<IEntity, IReadOnlyList<IComponent>> Spawned;

        event Action<IEntity, IReadOnlyList<IComponent>> Recycled;

        event Action<IEntity, IReadOnlyList<IComponent>> CleanedUp;

        void Load(IEntity prefab, int count = 1);

        TEntity Spawn<TEntity>(
            TEntity prefab,
            Vector3 position = default,
            Quaternion rotation = default,
            Transform? parent = null,
            bool spawnInWorldSpace = true
        )
            where TEntity : Component, IEntity;

        TEntity Spawn<TEntity, TParams>(
            TEntity prefab,
            TParams @params,
            Vector3 position = default,
            Quaternion rotation = default,
            Transform? parent = null,
            bool spawnInWorldSpace = true
        )
            where TEntity : Component, IEntity, IEntityWithParams<TParams>;

        void Recycle(IEntity instance);

        void RecycleAll(IEntity prefab);

        void Cleanup(IEntity prefab, int retainCount = 1);

        void Unload(IEntity prefab);

        IEnumerable<T> Query<T>() where T : IComponent;

        IEnumerable<IEntity> QueryEntities();
    }
}