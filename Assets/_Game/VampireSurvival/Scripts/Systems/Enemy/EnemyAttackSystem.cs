#nullable enable
using System;
using System.Collections.Generic;
using Core.Entities;
using UnityEngine;
using VampireSurvival.Core.Abstractions;
using VampireSurvival.Core.Stats;

namespace VampireSurvival.Core.Systems
{
    public sealed class EnemyAttackSystem : ISystem, IDisposable
    {
        private readonly IEntityManager entityManager;
        private readonly Dictionary<IEntity, float> cooldownByEnemy = new();

        private const float ATTACK_RANGE = 0.6f;
        private const float ATTACK_RANGE_SQ = ATTACK_RANGE * ATTACK_RANGE;
        private const float COOLDOWN = 0.4f;

        public EnemyAttackSystem(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
            this.entityManager.Recycled += this.OnRecycled;
        }

        void IDisposable.Dispose()
        {
            this.entityManager.Recycled -= this.OnRecycled;
            this.cooldownByEnemy.Clear();
        }

        private void OnRecycled(IEntity entity, IReadOnlyList<IComponent> _)
        {
            this.cooldownByEnemy.Remove(entity);
        }

        public void Tick(float dt)
        {
            IPlayer? player = null;
            foreach (var p in this.entityManager.Query<IPlayer>())
            {
                player = p;
                break;
            }
            if (player == null) return;

            var playerPos = (Vector2)player.transform.position;

            foreach (var damageStat in this.entityManager.Query<IDamageStat>())
            {
                var enemy = damageStat.Entity;

                if (!enemy.gameObject.TryGetComponent<IEnemy>(out _))
                    continue;

                this.cooldownByEnemy.TryGetValue(enemy, out var cooldown);
                cooldown -= dt;
                if (cooldown > 0f)
                {
                    this.cooldownByEnemy[enemy] = cooldown;
                    continue;
                }

                var enemyPos = (Vector2)enemy.transform.position;
                var distSq = (playerPos - enemyPos).sqrMagnitude;

                if (distSq <= ATTACK_RANGE_SQ)
                {
                    player.HealthStat.TakeDamage(damageStat.Value);
                    this.cooldownByEnemy[enemy] = COOLDOWN;
                }
            }
        }
    }
}