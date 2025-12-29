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
        public T    Open<T>(T prefab, RectTransform parent) where T : MonoBehaviour, IScreenView;
        public void Close<T>() where T : IScreenView;
        public void CloseAll();
    }

    public sealed class ScreenManager : IScreenManager, IDisposable
    {
        private readonly IDependencyContainer                                      container;
        private readonly IObjectPoolManager                                        objectPoolManager;
        private readonly Dictionary<Type, (IScreenView screen, GameObject prefab)> openScreens = new();

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

        public T Open<T>(T prefab, RectTransform parent) where T : MonoBehaviour, IScreenView
        {
            var type = typeof(T);

            if (this.openScreens.TryGetValue(type, out var entry))
            {
                entry.screen.Close();
                this.objectPoolManager.Recycle(entry.screen.GameObject);
                this.openScreens.Remove(type);
            }

            var go = this.objectPoolManager.Spawn(prefab.gameObject, parent: parent);
            go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            var screen = go.GetComponent<T>();

            this.openScreens[type] = (screen, prefab.gameObject);
            screen.Open();

            return screen;
        }

        public void Close<T>() where T : IScreenView
        {
            var type = typeof(T);

            if (!this.openScreens.TryGetValue(type, out var entry)) return;

            entry.screen.Close();
            this.objectPoolManager.Recycle(entry.screen.GameObject);
            this.openScreens.Remove(type);
        }

        public void CloseAll()
        {
            foreach (var entry in this.openScreens.Values)
            {
                entry.screen.Close();
                this.objectPoolManager.Recycle(entry.screen.GameObject);
            }

            this.openScreens.Clear();
        }

        void IDisposable.Dispose()
        {
            this.objectPoolManager.Instantiated -= this.OnInstantiated;
        }
    }
}