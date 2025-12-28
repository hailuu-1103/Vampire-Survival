#nullable enable
using Core.Entities;
using UnityEngine;

namespace VampireSurvival.Core
{
    public sealed class MovementSystem
    {
        private readonly IEntityManager entityManager;

        public MovementSystem(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void Tick()
        {
            var move = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (move.sqrMagnitude > 1f) move.Normalize();

            foreach (var m in this.entityManager.Query<IMoveable>())
            {
                m.Move(move);
            }
        }
    }
}