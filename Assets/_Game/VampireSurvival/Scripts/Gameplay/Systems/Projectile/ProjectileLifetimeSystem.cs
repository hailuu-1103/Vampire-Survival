#nullable enable

using Core.Entities;

namespace VampireSurvival.Systems
{
    using System.Collections.Generic;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Configs;

    public sealed class ProjectileLifetimeSystem : System<IProjectile>
    {
        private readonly Dictionary<IProjectile, float> spawnTimes = new();

        private ProjectileConfig config = null!;

        protected override void OnInstantiate()
        {
            this.config = this.Container.Resolve<ProjectileConfig>();
        }

        protected override void OnEntitySpawned(IEntity entity, IReadOnlyList<IComponent> components)
        {
            base.OnEntitySpawned(entity, components);

            if (entity is IProjectile projectile)
                this.spawnTimes[projectile] = Time.time;

            foreach (var component in components)
            {
                if (component is IProjectile proj)
                    this.spawnTimes[proj] = Time.time;
            }
        }

        protected override void OnEntityRecycled(IEntity entity, IReadOnlyList<IComponent> components)
        {
            base.OnEntityRecycled(entity, components);

            if (entity is IProjectile projectile)
                this.spawnTimes.Remove(projectile);

            foreach (var component in components)
            {
                if (component is IProjectile proj)
                    this.spawnTimes.Remove(proj);
            }
        }

        protected override bool Filter(IProjectile projectile) => true;

        protected override void Apply(IProjectile projectile)
        {
            if (!this.spawnTimes.TryGetValue(projectile, out var spawnTime)) return;

            if (Time.time - spawnTime < this.config.lifetime) return;

            this.Manager.Recycle(projectile);
        }
    }
}
