#nullable enable

namespace VampireSurvival.Core.Systems
{
    using IEventBus = global::Core.Observer.IEventBus;
    using System.Linq;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Events;
    using VampireSurvival.Core.Models;

    public sealed class PlayerAttackSystem : System<IPlayer>
    {
        private IEventBus eventBus = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
        }

        private float cooldown;

        protected override bool Filter(IPlayer player)
        {
            return player.StatsHolder.Stats[StatNames.HEALTH].Value > 0;
        }

        protected override void Apply(IPlayer player)
        {
            this.cooldown -= Time.deltaTime;
            if (this.cooldown > 0f) return;

            var attackRange = player.StatsHolder.Stats[StatNames.ATTACK_RANGE].Value;

            foreach (var enemy in this.Manager.Query<IEnemy>().ToArray())
            {
                var enemyHealth = enemy.StatsHolder.Stats[StatNames.HEALTH].Value;
                if (enemyHealth <= 0) continue;

                var distance = player.Collider.Distance(enemy.Collider);
                if (!distance.isValid || distance.distance > attackRange) continue;

                var dir           = enemy.transform.position.x - player.transform.position.x;
                var isFacingEnemy = (player.Animation.FacingDirection > 0 && dir > 0) || (player.Animation.FacingDirection < 0 && dir < 0);
                if (!isFacingEnemy) continue;

                var damage = player.StatsHolder.Stats[StatNames.ATTACK].Value;
                enemy.StatsHolder.Add(StatNames.HEALTH, -damage);

                player.Animation.PlayAttackAnimation();
                enemy.Animation.PlayHitAnimation();

                if (enemy.StatsHolder.Stats[StatNames.HEALTH].Value <= 0)
                {
                    this.eventBus.Publish(new EnemyDiedEvent(enemy));
                }

                this.cooldown = player.StatsHolder.Stats[StatNames.ATTACK_COOLDOWN].Value;
                break;
            }
        }
    }
}