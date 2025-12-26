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

        public GameObject Spawn(GameObject prefab, Vector3 position = default, Quaternion rotation = default, Transform? parent = null, bool spawnInWorldSpace = true);

        public void Recycle(GameObject instance);

        public void RecycleAll(GameObject prefab);


        public void Cleanup(GameObject prefab, int retainCount = 1);


        public void Unload(GameObject prefab);

        #region Component

        public void Load(Component prefab, int count = 1) => this.Load(prefab.gameObject, count);

        public T Spawn<T>(T prefab, Vector3 position = default, Quaternion rotation = default, Transform? parent = null, bool spawnInWorldSpace = true) where T : Component => this.Spawn(prefab.gameObject, position, rotation, parent, spawnInWorldSpace).GetComponent<T>();

        public void Recycle(Component instance) => this.Recycle(instance.gameObject);

        public void RecycleAll(Component prefab) => this.RecycleAll(prefab.gameObject);

        public void Cleanup(Component prefab, int retainCount = 1) => this.Cleanup(prefab.gameObject, retainCount);

        public void Unload(Component prefab) => this.Unload(prefab.gameObject);

        #endregion
    }
}