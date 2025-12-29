#nullable enable

using Entity = Core.Entities.Entity;
using IEventBus = Core.Observer.IEventBus;

namespace VampireSurvival.Core.Entities
{
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Events;
    using VampireSurvival.Core.Stats;

    public sealed class Player : Entity, IPlayer
    {
        private IEventBus eventBus = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
        }

        public IPlayerAnimation Animation  => this.GetComponent<IPlayerAnimation>();
        public IPlayerMoveable  Movement   => this.GetComponent<IPlayerMoveable>();
        public IStats           Stats      => this.GetComponent<IStats>();
        public IHealthStat      HealthStat => this.GetComponent<IHealthStat>();
        public Collider2D       Collider   => this.GetComponent<Collider2D>();

        protected override void OnSpawn()
        {
            this.HealthStat.Died += this.OnDied;
            this.Animation.PlayIdleAnimation();
        }

        private void OnDied()
        {
            this.eventBus.Publish<PlayerDiedEvent>(new());
        }

        protected override void OnRecycle()
        {
            this.HealthStat.Died -= this.OnDied;
        }
    }
}