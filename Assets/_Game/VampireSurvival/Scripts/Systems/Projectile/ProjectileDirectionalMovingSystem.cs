#nullable enable
using Core.Entities;

namespace VampireSurvival.Systems
{
    using UnityEngine;
    using VampireSurvival.Abstractions;

    public class ProjectileDirectionalMovingSystem : System<IProjectile>
    {
        protected override bool Filter(IProjectile entity)
        {
            if (!entity.Data.Direction.HasValue) return false;
            return entity.Target is null;
        }

        protected override void Apply(IProjectile entity)
        {
            var speed     = entity.Data.Speed;
            var direction = (Vector3)entity.Data.Direction!.Value;
            entity.transform.position += direction.normalized * speed * Time.deltaTime;
            entity.transform.Rotate(entity.Owner.transform.forward, speed * 100 * Time.deltaTime);
        }
    }
}