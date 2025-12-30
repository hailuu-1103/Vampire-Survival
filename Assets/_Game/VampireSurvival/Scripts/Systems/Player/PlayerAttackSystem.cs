#nullable enable
using System.Linq;
using Core.Entities;
using VampireSurvival.Core.Abstractions;

namespace VampireSurvival.Core.Systems
{
    public sealed class PlayerAttackSystem : IUpdateable
    {
        private readonly IEntityManager entityManager;

        private const float ATTACK_RANGE = 0.2f;
        private const float COOLDOWN     = 1f;

        private float cooldown;
        private bool  isPaused;

        public PlayerAttackSystem(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void Pause()  => this.isPaused = true;
        public void Resume() => this.isPaused = false;

        public void Tick(float deltaTime)
        {
            if (this.isPaused) return;

            this.cooldown -= deltaTime;
            if (this.cooldown > 0f) return;

            var player = this.entityManager.Query<IPlayer>().Single();
            if (player.Collider == null) return;

            foreach (var enemy in this.entityManager.Query<IEnemy>().ToList())
            {
                if (enemy.Collider == null) continue;

                var distance = player.Collider.Distance(enemy.Collider);
                if (!distance.isValid || distance.distance > ATTACK_RANGE) continue;
                if (enemy.HealthStat.Current <= 0) continue;

                var dir = enemy.transform.position.x - player.transform.position.x;
                var isFacingEnemy = (player.Animation.FacingDirection > 0 && dir > 0) ||
                                    (player.Animation.FacingDirection < 0 && dir < 0);
                if (!isFacingEnemy) continue;

                player.Animation.PlayAttackAnimation();
                enemy.Animation.PlayHitAnimation();
                enemy.HealthStat.TakeDamage(player.DamageStat.Value);
                this.cooldown = COOLDOWN;
                break;
            }
        }
    }
}