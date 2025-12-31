#nullable enable
namespace Core.Pooling
{
    using System;
    using UnityEngine;

    public interface IObjectPoolManager
    {
        public event Action<GameObject> Instantiated;

        public event Action<GameObject> Spawned;

        public event Action<GameObject> Recycled;

        public event Action<GameObject> CleanedUp;

        public void Load(GameObject prefab, int count = 1);

        public void Load<T>(int count = 1) where T : Component;

        public void Load<T>(string resourcePath, int count = 1) where T : Component;

        public GameObject Spawn(GameObject prefab, Vector3 position = default, Quaternion rotation = default, Transform? parent = null, bool spawnInWorldSpace = true);

        public T Spawn<T>(Vector3 position = default, Quaternion rotation = default, Transform? parent = null, bool spawnInWorldSpace = true) where T : Component;

        public void Recycle(GameObject instance);

        public void RecycleAll(GameObject prefab);

        public void RecycleAll<T>() where T : Component;

        public void Cleanup(GameObject prefab, int retainCount = 1);

        public void Cleanup<T>(int retainCount = 1) where T : Component;

        public void Unload(GameObject prefab);

        public void Unload<T>() where T : Component;
    }
}