#nullable enable

namespace VampireSurvival.Core.Systems
{
    using IEventBus     = global::Core.Observer.IEventBus;
    using ILateLoadable = global::Core.Lifecycle.ILateLoadable;
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