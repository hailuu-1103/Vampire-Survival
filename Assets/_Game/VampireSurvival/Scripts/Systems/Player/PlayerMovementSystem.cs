#nullable enable
using Core.Entities;
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;

    public sealed class PlayerMovementSystem : ISystem
    {
        private readonly IEntityManager entityManager;

        public PlayerMovementSystem(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void Tick(float deltaTime)
        {
            var move = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (move.sqrMagnitude > 1f) move.Normalize();

            foreach (var m in this.entityManager.Query<IPlayerMoveable>())
            {
                m.Move(move);
            }
        }
    }
}