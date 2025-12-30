#nullable enable
using System.Linq;
using Core.Entities;
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;

    public sealed class PlayerMovementSystem : IUpdateable
    {
        private readonly IEntityManager entityManager;
        private bool isPaused;

        public PlayerMovementSystem(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void Pause()  => this.isPaused = true;
        public void Resume() => this.isPaused = false;

        public void Tick(float deltaTime)
        {
            if (this.isPaused) return;

            var player = this.entityManager.Query<IPlayer>().Single();
            if (!player.Animation.CanMove)
            {
                player.Movement.Move(Vector2.zero);
                return;
            }

            var move = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (move.sqrMagnitude > 1f) move.Normalize();

            player.Movement.Move(move);

            var isMoving = move.sqrMagnitude > 0.01f;

            if (isMoving)
            {
                player.Animation.SetFacing(move.x);
                player.Animation.PlayRunAnimation();
            }
            else
            {
                player.Animation.PlayIdleAnimation();
            }
        }
    }
}