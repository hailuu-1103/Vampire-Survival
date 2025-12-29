#nullable enable
namespace VampireSurvival.Core.Abstractions
{
    using UnityEngine;

    public interface IMoveable
    {
        public Vector2 Position { get; }
        public void    Move(Vector2 move);
    }

    public interface IPlayerMoveable : IMoveable { }

    public interface IEnemyMoveable : IMoveable { }
}