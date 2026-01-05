#nullable enable

using Core.Entities;
using Core.Observer;

namespace VampireSurvival.Systems
{
    using System.Linq;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Configs;
    using VampireSurvival.Events;
    using VampireSurvival.Models;

    public sealed class ProjectileAttackingSystem : System<IProjectile>
    {
        private const float COLLISION_PADDING = 0.5f;

        private IEventBus        eventBus = null!;
        private ProjectileConfig config   = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
            this.config   = this.Container.Resolve<ProjectileConfig>();
        }

        protected override bool Filter(IProjectile projectile) => true;

        protected override void Apply(IProjectile projectile)
        {
            this.ApplyAttackEnemy(projectile);
        }

        private void ApplyAttackEnemy(IProjectile projectile)
        {
            var position  = (Vector2)projectile.transform.position;
            var hitRadius = this.config.hitRadius;

            foreach (var enemy in this.Manager.Query<IEnemy>().ToArray())
            {
                if (ReferenceEquals(projectile.Owner, enemy)) continue;
                if (!enemy.IsAlive) continue;

                var distance = Vector2.Distance(position, (Vector2)enemy.transform.position);
                if (distance > hitRadius + COLLISION_PADDING) continue;

                var colliderDistance = projectile.Collider.Distance(enemy.Collider);
                if (!colliderDistance.isValid || colliderDistance.distance > 0) continue;

                this.ApplyDamageToEnemy(projectile, enemy);
                this.Manager.Recycle(projectile);
                return;
            }
        }

        private void ApplyDamageToEnemy(IProjectile projectile, IEnemy enemy)
        {
            enemy.StatsHolder.Add(CharacterStatNames.HEALTH, -projectile.Data.Damage);
            enemy.Animation.SetAnimation(AnimationType.Hit);
            if (!enemy.IsAlive) this.eventBus.Publish(new EnemyDiedEvent(enemy));
        }
    }
}