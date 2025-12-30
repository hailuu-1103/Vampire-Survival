#nullable enable
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using UnityEngine;
using VampireSurvival.Core.Abstractions;

namespace VampireSurvival.Core.Systems
{
    using System;

    public sealed class EnemyAttackSystem : IUpdateable, IDisposable
    {
        private readonly IEntityManager            entityManager;
        private readonly Dictionary<IEnemy, float> cooldowns = new();

        private const float ATTACK_RANGE = 0.15f;
        private const float COOLDOWN     = 2f;

        private bool isPaused;

        public EnemyAttackSystem(IEntityManager entityManager)
        {
            this.entityManager          =  entityManager;
            this.entityManager.Spawned  += this.OnSpawned;
            this.entityManager.Recycled += this.OnRecycled;
        }

        public void Pause() => this.isPaused = true;

        public void Resume() => this.isPaused = false;

        private void OnSpawned(IEntity entity, IReadOnlyList<IComponent> arg2)
        {
            if (entity is IEnemy enemy) this.cooldowns.TryAdd(enemy, 0);
        }

        private void OnRecycled(IEntity entity, IReadOnlyList<IComponent> _)
        {
            if (entity is IEnemy enemy) this.cooldowns.Remove(enemy);
        }

        public void Tick(float deltaTime)
        {
            if (this.isPaused) return;

            var player = this.entityManager.Query<IPlayer>().SingleOrDefault();
            if (player?.Collider == null) return;

            foreach (var enemy in this.entityManager.Query<IEnemy>().ToList())
            {
                if (!this.cooldowns.TryGetValue(enemy, out var cooldown)) continue;
                if (enemy.Collider == null) continue;

                cooldown              -= deltaTime;
                this.cooldowns[enemy] =  Mathf.Max(0, cooldown);
                if (cooldown > 0f) continue;

                var distance = enemy.Collider.Distance(player.Collider);
                if (!distance.isValid || distance.distance > ATTACK_RANGE) continue;
                if (player.HealthStat.Current <= 0) continue;

                var dir = player.transform.position.x - enemy.transform.position.x;
                var isFacingPlayer = (enemy.Animation.FacingDirection > 0 && dir > 0) ||
                                     (enemy.Animation.FacingDirection < 0 && dir < 0);
                if (!isFacingPlayer) continue;

                enemy.Animation.PlayAttackAnimation();
                player.Animation.PlayHitAnimation();
                player.HealthStat.TakeDamage(enemy.DamageStat.Value);
                this.cooldowns[enemy] = COOLDOWN;
            }
        }

        void IDisposable.Dispose()
        {
            this.entityManager.Recycled -= this.OnRecycled;
            this.entityManager.Spawned  -= this.OnSpawned;
        }
    }
}