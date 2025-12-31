#nullable enable
namespace Core.ScreenFlow
{
    using System;
    using System.Collections.Generic;
    using Core.DI;
    using Core.Pooling;
    using UnityEngine;

    public interface IScreenManager
    {
        public IScreenView? CurrentActiveScreen { get; }
        public void         Load<T>(T prefab) where T : MonoBehaviour, IScreenView;
        public T            Open<T>(T prefab, RectTransform parent) where T : MonoBehaviour, IScreenView;
        public void         Close<T>() where T : IScreenView;
        public void         CloseAll();
    }

    public sealed class ScreenManager : IScreenManager, IDisposable
    {
        private readonly IDependencyContainer                                      container;
        private readonly IObjectPoolManager                                        objectPoolManager;
        private readonly Dictionary<Type, (IScreenView screen, GameObject prefab)> screens = new();

        private IScreenView? currentActiveScreen;
        public  IScreenView? CurrentActiveScreen => this.currentActiveScreen;

        public ScreenManager(IDependencyContainer container, IObjectPoolManager objectPoolManager)
        {
            this.container         = container;
            this.objectPoolManager = objectPoolManager;

            this.objectPoolManager.Instantiated += this.OnInstantiated;
        }

        private void OnInstantiated(GameObject instance)
        {
            if (instance.GetComponent<IScreenView>() == null) return;
            this.container.InjectGameObject(instance);
        }

        public void Load<T>(T prefab) where T : MonoBehaviour, IScreenView
        {
            this.objectPoolManager.Load(prefab.gameObject);
        }

        public T Open<T>(T prefab, RectTransform parent) where T : MonoBehaviour, IScreenView
        {
            var type = typeof(T);

            if (this.screens.TryGetValue(type, out var entry))
            {
                entry.screen.Open();
                this.currentActiveScreen = entry.screen;
                return (T)entry.screen;
            }

            var go     = this.objectPoolManager.Spawn(prefab.gameObject, parent: parent, spawnInWorldSpace: false);
            var screen = go.GetComponent<T>();

            this.screens[type] = (screen, prefab.gameObject);
            screen.Open();
            this.currentActiveScreen = screen;
            return screen;
        }

        public void Close<T>() where T : IScreenView
        {
            var type = typeof(T);

            if (!this.screens.TryGetValue(type, out var entry)) return;

            entry.screen.Close();
            this.currentActiveScreen = null;
        }

        public void CloseAll()
        {
            foreach (var entry in this.screens.Values)
            {
                entry.screen.Close();
            }
            this.currentActiveScreen = null;
        }

        void IDisposable.Dispose()
        {
            foreach (var entry in this.screens.Values)
            {
                this.objectPoolManager.Recycle(entry.screen.GameObject);
            }
            this.screens.Clear();
            this.objectPoolManager.Instantiated -= this.OnInstantiated;
        }
    }
}