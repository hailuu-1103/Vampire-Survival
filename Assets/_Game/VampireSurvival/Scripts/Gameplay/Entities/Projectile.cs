#nullable enable

using IEntity = Core.Entities.IEntity;
using Core.Entities;

namespace VampireSurvival.Entities
{
    using UnityEngine;
    using VampireSurvival.Abstractions;

    public sealed record ProjectileParams(
        float    Damage,
        float    Speed,
        Vector2  Direction,
        IEntity  Owner,
        IEntity? Target = null
    );

    [RequireComponent(typeof(Collider2D))]
    public sealed class Projectile : Entity<ProjectileParams>, IProjectile
    {
        private Collider2D col = null!;

        protected override void OnInstantiate()
        {
            this.col = this.GetComponent<Collider2D>();
        }

        Collider2D IHasCollider.Collider => this.col;
        public IEntity          Owner    => this.Params.Owner;
        public IEntity?         Target   => this.Params.Target;
        public ProjectileData   Data     => new(this.Params.Damage, this.Params.Speed, this.Params.Direction);

        protected override void OnSpawn()
        {
            this.UpdateRotation();
        }

        private void UpdateRotation()
        {
            var direction = this.Params.Direction;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}