#nullable enable

namespace Game.Services
{
    using System;
    using Core.Lifecycle;
    using Core.Observer;
    using VampireSurvival.Core.Events;

    public class KillCountTracker : ILateLoadable, IDisposable
    {
        private readonly IEventBus eventBus;

        public KillCountTracker(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public  Action? onKillChanged;
        public  int     KillCounts => this.deadCounts;
        private int     deadCounts;

        void ILateLoadable.OnLateLoad()
        {
            this.eventBus.Subscribe<EnemyDiedEvent>(this.OnEnemyDied);
            this.eventBus.Subscribe<LostEvent>(this.OnReset);
            this.eventBus.Subscribe<WonEvent>(this.OnReset);
        }

        private void OnReset<T>(T _)
        {
            this.deadCounts = 0;
        }

        private void OnEnemyDied(EnemyDiedEvent obj)
        {
            this.deadCounts++;
            this.onKillChanged?.Invoke();

            if (this.deadCounts < 100) return;
            this.eventBus.Publish(new WonEvent());
        }

        void IDisposable.Dispose()
        {
            this.eventBus.Unsubscribe<EnemyDiedEvent>(this.OnEnemyDied);
            this.eventBus.Unsubscribe<LostEvent>(this.OnReset);
            this.eventBus.Unsubscribe<WonEvent>(this.OnReset);
        }
    }
}