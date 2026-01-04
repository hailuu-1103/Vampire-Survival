#nullable enable

namespace Core.Entities
{
    using Core.Lifecycle;
    using Core.Observer;
    using System;

    public interface IReactiveSystem { }

    public abstract class ReactiveSystem<TEvent> : IReactiveSystem, ILateLoadable, IDisposable
    {
        protected readonly IEventBus eventBus;

        protected ReactiveSystem(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        void ILateLoadable.OnLateLoad()
        {
            this.eventBus.Subscribe<TEvent>(this.OnEvent);
            this.OnLoad();
        }

        void IDisposable.Dispose()
        {
            this.eventBus.Unsubscribe<TEvent>(this.OnEvent);
            this.OnDispose();
        }

        protected abstract void OnEvent(TEvent e);

        protected virtual void OnLoad() { }

        protected virtual void OnDispose() { }
    }
}
