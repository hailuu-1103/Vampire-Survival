#nullable enable

namespace VampireSurvival.Abstractions
{
    using UnityEngine;
    using IEntity = global::Core.Entities.IEntity;

    public sealed record ProjectileData(
        float    Damage,
        float    Speed,
        Vector2? Direction
    );

    public interface IProjectile : IEntity, IHasCollider
    {
        public IEntity        Owner           { get; }
        public IEntity?       Target          { get; }
        public ProjectileData Data            { get; }
    }
}