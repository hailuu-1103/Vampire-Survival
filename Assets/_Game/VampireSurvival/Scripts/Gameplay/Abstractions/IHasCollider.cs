#nullable enable

namespace VampireSurvival.Abstractions
{
    using UnityEngine;

    public interface IHasCollider
    {
        public Collider2D Collider { get; }
    }

    public interface IHasRigidbody
    {
        public Rigidbody2D Rigidbody { get; }
    }
}
