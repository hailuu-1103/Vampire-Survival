#nullable enable

namespace VampireSurvival.Components
{
    using Component  = global::Core.Entities.Component;
    using IEventBus  = global::Core.Observer.IEventBus;
    using IPauseable = global::Core.Entities.IPauseable;
    using UnityEngine;
    using VampireSurvival.Entities;
    using VampireSurvival.Events;
    using VampireSurvival.Configs;

    public sealed class XPCollectableSpawner : Component, IPauseable
    {
        [SerializeField] private XPCollectible prefab = null!;

        private IEventBus   eventBus    = null!;
        private EnemyConfig enemyConfig = null!;
        private bool        isPaused;

        protected override void OnInstantiate()
        {
            base.OnInstantiate();
            this.eventBus    = this.Container.Resolve<IEventBus>();
            this.enemyConfig = this.Container.Resolve<EnemyConfig>();
            this.Manager.Load(this.prefab, 20);
        }

        protected override void OnSpawn()
        {
            this.isPaused = false;
            this.eventBus.Subscribe<EnemyDiedEvent>(this.OnEnemyDied);
        }

        private void OnEnemyDied(EnemyDiedEvent obj)
        {
            if (this.isPaused) return;

            var position = obj.Enemy.transform.position;
            var xpValue  = this.enemyConfig.xpReward;

            this.Manager.Spawn(
                this.prefab,
                new XPCollectableParams(xpValue),
                position,
                Quaternion.identity
            );
        }

        void IPauseable.Pause()  => this.isPaused = true;
        void IPauseable.Resume() => this.isPaused = false;

        protected override void OnRecycle()
        {
            this.eventBus.Unsubscribe<EnemyDiedEvent>(this.OnEnemyDied);
        }
    }
}
