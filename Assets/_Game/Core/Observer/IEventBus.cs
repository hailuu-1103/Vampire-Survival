#nullable enable

namespace Core.Observer
{
    using System;

    public interface IEventBus
    {
        public void Publish<T>(T             signal);
        public void Subscribe<T>(Action<T>   handler);
        public void Unsubscribe<T>(Action<T> handler);
    }
}