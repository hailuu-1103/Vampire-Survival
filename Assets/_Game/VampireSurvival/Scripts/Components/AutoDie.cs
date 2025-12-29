#nullable enable

using Entities_Component = Core.Entities.Component;
using IEventBus = Core.Observer.IEventBus;

namespace VampireSurvival.Core.Components
{
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Events;

    public class AutoDie : Entities_Component, IPauseable
    {
        [SerializeField] private int experience;

        private IEventBus eventBus = null!;

        private const float LIFETIME_SECONDS = 10f;
        private       float timer;
        private       bool  isPaused;

        protected override void OnInstantiate()
        {
            base.OnInstantiate();
            this.eventBus = this.Container.Resolve<IEventBus>();
        }

        protected override void OnSpawn()
        {
            this.timer = LIFETIME_SECONDS;
        }

        private void Update()
        {
            if (this.isPaused) return;
            this.timer -= Time.deltaTime;
            if (this.timer > 0f) return;

            this.eventBus.Publish(new EnemyDiedEvent(this.experience));
            this.Manager.Recycle(this.Entity);
        }

        void IPauseable.Pause() => this.isPaused = true;

        void IPauseable.Resume() => this.isPaused = false;
    }
}