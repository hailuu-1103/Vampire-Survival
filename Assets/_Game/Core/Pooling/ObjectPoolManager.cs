#nullable enable
namespace Core.Pooling
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class ObjectPoolManager : IObjectPoolManager
    {
        public event Action<GameObject>? Instantiated;
        public event Action<GameObject>? Spawned;
        public event Action<GameObject>? Recycled;
        public event Action<GameObject>? CleanedUp;

        private readonly Dictionary<GameObject, ObjectPool> pools = new();

        private readonly Transform root;

        public ObjectPoolManager()
        {
            var rootGO = new GameObject("ObjectPools");
            GameObject.DontDestroyOnLoad(rootGO);
            this.root = rootGO.transform;
        }

        public void Load(GameObject prefab, int count = 1)
        {
            var pool = this.GetOrCreatePool(prefab);
            pool.Load(count);
        }

        public GameObject Spawn(
            GameObject prefab,
            Vector3 position = default,
            Quaternion rotation = default,
            Transform? parent = null,
            bool spawnInWorldSpace = true)
        {
            var pool = this.GetOrCreatePool(prefab);
            return pool.Spawn(position, rotation, parent, spawnInWorldSpace);
        }

        public void Recycle(GameObject instance)
        {
            if (instance == null)
                return;

            var pool = instance.GetComponentInParent<ObjectPool>();
            if (pool == null)
            {
                Debug.LogWarning($"[ObjectPoolManager] No ObjectPool found for {instance.name}");
                return;
            }

            pool.Recycle(instance);
        }

        public void RecycleAll(GameObject prefab)
        {
            if (!this.pools.TryGetValue(prefab, out var pool))
                return;

            pool.RecycleAll();
        }

        public void Cleanup(GameObject prefab, int retainCount = 1)
        {
            if (!this.pools.TryGetValue(prefab, out var pool))
                return;

            pool.Cleanup(retainCount);
        }


        public void Unload(GameObject prefab)
        {
            if (!this.pools.TryGetValue(prefab, out var pool))
                return;

            UnityEngine.Object.Destroy(pool.gameObject);
            this.pools.Remove(prefab);
        }

        private ObjectPool GetOrCreatePool(GameObject prefab)
        {
            if (this.pools.TryGetValue(prefab, out var pool))
                return pool;

            pool = ObjectPool.Construct(prefab, this.root);

            // Forward events
            pool.Instantiated += go => this.Instantiated?.Invoke(go);
            pool.Spawned      += go => this.Spawned?.Invoke(go);
            pool.Recycled     += go => this.Recycled?.Invoke(go);
            pool.CleanedUp    += go => this.CleanedUp?.Invoke(go);

            this.pools[prefab] = pool;
            return pool;
        }
    }
}