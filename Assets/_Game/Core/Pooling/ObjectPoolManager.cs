#nullable enable
namespace Core.Pooling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core.Utils;
    using Core.Utils.TheOne.Extensions;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public sealed class ObjectPoolManager : IObjectPoolManager
    {
        #region Constructor

        private readonly Transform                          poolsContainer = new GameObject(nameof(ObjectPoolManager)).DontDestroyOnLoad().transform;
        private readonly Dictionary<GameObject, ObjectPool> prefabToPool   = new Dictionary<GameObject, ObjectPool>();
        private readonly Dictionary<GameObject, ObjectPool> instanceToPool = new Dictionary<GameObject, ObjectPool>();
        private readonly Dictionary<Type, GameObject>       typeToPrefab   = new Dictionary<Type, GameObject>();

        #endregion

        #region Public

        event Action<GameObject> IObjectPoolManager.Instantiated { add => this.instantiated += value; remove => this.instantiated -= value; }
        event Action<GameObject> IObjectPoolManager.Spawned      { add => this.spawned += value;      remove => this.spawned -= value; }
        event Action<GameObject> IObjectPoolManager.Recycled     { add => this.recycled += value;     remove => this.recycled -= value; }
        event Action<GameObject> IObjectPoolManager.CleanedUp    { add => this.cleanedUp += value;    remove => this.cleanedUp -= value; }

        void IObjectPoolManager.Load(GameObject prefab, int count) => this.Load(prefab, count);

        void IObjectPoolManager.Load<T>(int count) => this.Load<T>(typeof(T).Name, count);

        void IObjectPoolManager.Load<T>(string resourcePath, int count) => this.Load<T>(resourcePath, count);

        GameObject IObjectPoolManager.Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform? parent, bool spawnInWorldSpace) => this.Spawn(prefab, position, rotation, parent, spawnInWorldSpace);

        T IObjectPoolManager.Spawn<T>(Vector3 position, Quaternion rotation, Transform? parent, bool spawnInWorldSpace) => this.Spawn<T>(position, rotation, parent, spawnInWorldSpace);

        void IObjectPoolManager.Recycle(GameObject instance)
        {
            if (!this.instanceToPool.Remove(instance, out var pool)) throw new InvalidOperationException($"{instance.name} was not spawned from {nameof(ObjectPoolManager)}");
            pool.Recycle(instance);
            Debug.Log($"Recycled {instance.name}");
        }

        void IObjectPoolManager.RecycleAll(GameObject prefab) => this.RecycleAll(prefab);

        void IObjectPoolManager.RecycleAll<T>() => this.RecycleAll<T>();

        void IObjectPoolManager.Cleanup(GameObject prefab, int retainCount) => this.Cleanup(prefab, retainCount);

        void IObjectPoolManager.Cleanup<T>(int retainCount) => this.Cleanup<T>(retainCount);

        void IObjectPoolManager.Unload(GameObject prefab) => this.Unload(prefab);

        void IObjectPoolManager.Unload<T>() => this.Unload<T>();

        #endregion

        #region Private

        private Action<GameObject>? instantiated;
        private Action<GameObject>? spawned;
        private Action<GameObject>? recycled;
        private Action<GameObject>? cleanedUp;

        private void Load(GameObject prefab, int count)
        {
            this.prefabToPool.GetOrAdd(prefab, () =>
            {
                var pool = ObjectPool.Construct(prefab, this.poolsContainer);
                pool.Instantiated += this.OnInstantiated;
                pool.Spawned      += this.OnSpawned;
                pool.Recycled     += this.OnRecycled;
                pool.CleanedUp    += this.OnCleanedUp;
                return pool;
            }).Load(count);
        }

        private void Load<T>(string resourcePath, int count) where T : Component
        {
            var type = typeof(T);
            if (this.typeToPrefab.ContainsKey(type)) return;

            var prefab = Resources.Load<T>(resourcePath);
            if (prefab == null)
            {
                Debug.LogError($"Failed to load {type.Name} from Resources/{resourcePath}");
                return;
            }

            this.typeToPrefab[type] = prefab.gameObject;
            this.Load(prefab.gameObject, count);
        }

        private GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform? parent, bool spawnInWorldSpace)
        {
            if (!this.prefabToPool.ContainsKey(prefab))
            {
                this.Load(prefab, 1);
                Debug.LogWarning($"Auto loaded {prefab.name} pool. Consider preload it with `Load` or `LoadAsync` for better performance.");
            }
            var pool     = this.prefabToPool[prefab];
            var instance = pool.Spawn(position, rotation, parent, spawnInWorldSpace);
            this.instanceToPool.Add(instance, pool);
            Debug.Log($"Spawned {instance.name}");
            return instance;
        }

        private T Spawn<T>(Vector3 position, Quaternion rotation, Transform? parent, bool spawnInWorldSpace) where T : Component
        {
            var type = typeof(T);
            if (!this.typeToPrefab.TryGetValue(type, out var prefab))
            {
                this.Load<T>(type.Name, 1);
                if (!this.typeToPrefab.TryGetValue(type, out prefab))
                    throw new InvalidOperationException($"Failed to load {type.Name} from Resources");
            }

            return this.Spawn(prefab, position, rotation, parent, spawnInWorldSpace).GetComponent<T>();
        }

        private void RecycleAll(GameObject prefab)
        {
            if (!this.TryGetPool(prefab, out var pool)) return;
            pool.RecycleAll();
            this.instanceToPool.RemoveWhere((_, otherPool) => otherPool == pool);
            Debug.Log($"Recycled all {pool.name}");
        }

        private void RecycleAll<T>() where T : Component
        {
            if (!this.TryGetPrefab<T>(out var prefab)) return;
            this.RecycleAll(prefab);
        }

        private void Cleanup(GameObject prefab, int retainCount)
        {
            if (!this.TryGetPool(prefab, out var pool)) return;
            pool.Cleanup(retainCount);
            Debug.Log($"Cleaned up {pool.name}");
        }

        private void Cleanup<T>(int retainCount) where T : Component
        {
            if (!this.TryGetPrefab<T>(out var prefab)) return;
            this.Cleanup(prefab, retainCount);
        }

        private void Unload(GameObject prefab)
        {
            if (!this.TryGetPool(prefab, out var pool)) return;
            this.RecycleAll(prefab);
            pool.Instantiated -= this.OnInstantiated;
            pool.Spawned      -= this.OnSpawned;
            pool.Recycled     -= this.OnRecycled;
            pool.CleanedUp    -= this.OnCleanedUp;
            Object.Destroy(pool.gameObject);
            this.prefabToPool.Remove(prefab);
            Debug.Log($"Destroyed {pool.name}");
        }

        private void Unload<T>() where T : Component
        {
            if (!this.TryGetPrefab<T>(out var prefab)) return;
            this.Unload(prefab);
            this.typeToPrefab.Remove(typeof(T));
        }

        private bool TryGetPool(GameObject prefab, [MaybeNullWhen(false)] out ObjectPool pool)
        {
            if (this.prefabToPool.TryGetValue(prefab, out pool)) return true;
            Debug.LogWarning($"{prefab.name} pool not loaded");
            return false;
        }

        private bool TryGetPrefab<T>([MaybeNullWhen(false)] out GameObject prefab) where T : Component
        {
            if (this.typeToPrefab.TryGetValue(typeof(T), out prefab)) return true;
            Debug.LogWarning($"{typeof(T).Name} pool not loaded");
            return false;
        }

        private void OnInstantiated(GameObject instance) => this.instantiated?.Invoke(instance);
        private void OnSpawned(GameObject      instance) => this.spawned?.Invoke(instance);
        private void OnRecycled(GameObject     instance) => this.recycled?.Invoke(instance);
        private void OnCleanedUp(GameObject    instance) => this.cleanedUp?.Invoke(instance);

        #endregion
    }
}