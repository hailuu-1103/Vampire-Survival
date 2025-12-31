#nullable enable

namespace Core.Observer
{
    using System;
    using System.Collections.Generic;

    public sealed class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> handlers = new();

        public void Publish<T>(T signal)
        {
            var type = typeof(T);

            if (!this.handlers.TryGetValue(type, out var list))
                return;

            foreach (var callback in list.ToArray())
            {
                ((Action<T>)callback)?.Invoke(signal);
            }
        }

        public void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);

            if (!this.handlers.TryGetValue(type, out var list))
            {
                list                = new();
                this.handlers[type] = list;
            }

            if (!list.Contains(handler))
                list.Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);

            if (!this.handlers.TryGetValue(type, out var list))
                return;

            list.Remove(handler);

            if (list.Count == 0) this.handlers.Remove(type);
        }
    }

}