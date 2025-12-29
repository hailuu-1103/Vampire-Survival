#nullable enable

using Entity = Core.Entities.Entity;
using IEventBus = Core.Observer.IEventBus;

namespace VampireSurvival.Core.Entities
{
    using Cysharp.Threading.Tasks;
    using Spine.Unity;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Events;
    using VampireSurvival.Core.Stats;

    public sealed class Enemy : Entity, IEnemy
    {
        private IEventBus eventBus = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
        }

        public IEnemyAnimation Animation  => this.GetComponent<IEnemyAnimation>();
        public IEnemyMoveable  Movement   => this.GetComponent<IEnemyMoveable>();
        public IStats          Stats      => this.GetComponent<IStats>();
        public IDamageStat     DamageStat => this.GetComponent<IDamageStat>();
        public IHealthStat     HealthStat => this.GetComponent<IHealthStat>();
        public Collider2D      Collider   => this.GetComponent<Collider2D>();

        protected override void OnSpawn()
        {
            this.Animation.SetColor(Color.red);
            this.HealthStat.Died += this.OnDied;
        }

        private void OnDied()
        {
            this.HandleDeathAsync().Forget();
        }

        private async UniTaskVoid HandleDeathAsync()
        {
            this.eventBus.Publish(new EnemyDiedEvent(5));
            await this.Animation.PlayDeathAnimationAsync();
            this.Manager.Recycle(this);
        }

        protected override void OnRecycle()
        {
            this.HealthStat.Died -= this.OnDied;
        }
    }
}