#nullable enable
using Entities_Component = Core.Entities.Component;
using IEntityManager = Core.Entities.IEntityManager;
using IEventBus = Core.Observer.IEventBus;

namespace VampireSurvival.Core.Components
{
    using UnityEngine;
    using VampireSurvival.Core.Entities;
    using VampireSurvival.Core.Events;
    using VampireSurvival.Core.Stats;

    public sealed class XPCollectableSpawner : Entities_Component
    {
        [SerializeField] private XPCollectible prefab = null!;

        private IEventBus      eventBus      = null!;
        private EnemyConfig    enemyConfig   = null!;

        protected override void OnInstantiate()
        {
            base.OnInstantiate();
            this.eventBus      = this.Container.Resolve<IEventBus>();
            this.enemyConfig   = this.Container.Resolve<EnemyConfig>();
            this.Manager.Load(this.prefab, 20);
        }

        protected override void OnSpawn()
        {
            this.eventBus.Subscribe<EnemyDiedEvent>(this.OnEnemyDied);
        }

        private void OnEnemyDied(EnemyDiedEvent obj)
        {
            var position = obj.Enemy.transform.position;
            var xpValue  = this.enemyConfig.xpReward;

            Debug.Log($"hehe: Spawning XPCollectable at {position}");

            var xp = this.Manager.Spawn(
                this.prefab,
                new XPCollectableParams(xpValue),
                position,
                Quaternion.identity
            );

            Debug.Log($"hehe: Spawned XPCollectable {xpValue}, active={xp.gameObject.activeSelf}, pos={xp.transform.position}");
        }

        protected override void OnRecycle()
        {
            this.eventBus.Unsubscribe<EnemyDiedEvent>(this.OnEnemyDied);
        }
    }
}