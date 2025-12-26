#nullable enable

namespace Core.Observer
{
    using System;

    public interface IEventBus
    {
        void Publish<T>(T             signal);
        void Subscribe<T>(Action<T>   handler);
        void Unsubscribe<T>(Action<T> handler);
    }

}