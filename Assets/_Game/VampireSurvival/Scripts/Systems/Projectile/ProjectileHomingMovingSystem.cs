#nullable enable

using Core.Entities;

namespace VampireSurvival.Systems
{
    using UnityEngine;
    using VampireSurvival.Abstractions;

    public sealed class ProjectileHomingMovingSystem : System<IProjectile>
    {
        protected override bool Filter(IProjectile entity)
        {
            if (entity.Target is not IEnemy enemy) return false;
            return enemy.IsAlive;
        }

        protected override void Apply(IProjectile entity)
        {
            var targetPosition = (Vector2)entity.Target!.transform.position;
            var speed          = entity.Data.Speed;
            entity.transform.position = Vector3.MoveTowards(
                entity.transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
            entity.transform.Rotate(entity.transform.forward, speed * 100 * Time.deltaTime);
        }
    }
}