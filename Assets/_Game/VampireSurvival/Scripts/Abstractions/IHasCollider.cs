#nullable enable
namespace VampireSurvival.Core.Abstractions
{
    using UnityEngine;

    public interface IHasCollider
    {
        public Collider2D Collider { get; }
    }
}