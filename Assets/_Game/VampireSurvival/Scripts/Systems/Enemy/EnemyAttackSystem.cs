#nullable enable
using Core.Entities;
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Stats;

    public sealed class EnemyAttackSystem
    {
        private readonly IEntityManager   entityManager = null!;

        private float cooldownTimer;

        //TODO
        private const float ATTACK_RANGE = 0.6f;

        public EnemyAttackSystem(IEntityManager entityManager)
        {
            this.entityManager    = entityManager;
        }

        public void Tick(float dt)
        {
            this.cooldownTimer -= dt;
            if (this.cooldownTimer > 0f) return;

            IPlayer? player = null;
            foreach (var p in this.entityManager.Query<IPlayer>())
            {
                player = p;
                break;
            }
            if (player == null) return;

            var playerHealth = player.gameObject.GetComponent<IHealthStat>();
            if (playerHealth == null) return;

            var playerPos = (Vector2)player.transform.position;

            foreach (var damageStat in this.entityManager.Query<IDamageStat>())
            {
                var entity = damageStat.Entity;
                if(!entity.gameObject.TryGetComponent<IEnemy>(out var enemyEntity)) continue;
                var enemyPos = (Vector2)enemyEntity.transform.position;
                var distSq   = (playerPos - enemyPos).sqrMagnitude;

                if (distSq <= ATTACK_RANGE)
                {
                    playerHealth.TakeDamage(damageStat.Value);
                    this.cooldownTimer = 0.4f;
                    return;
                }
            }
        }
    }
}