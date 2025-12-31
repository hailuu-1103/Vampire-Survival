#nullable enable
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Models;


    public sealed class PlayerMovementSystem : System<IPlayer>
    {
        protected override bool Filter(IPlayer player)
        {
            return player.StatsHolder.Stats[StatNames.HEALTH].Value > 0;
        }

        protected override void Apply(IPlayer player)
        {
            var move = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (!player.Animation.CanMove)
            {
                player.Rigidbody.linearVelocity = Vector2.zero;
                return;
            }

            if (move.sqrMagnitude > 1f) move.Normalize();

            var speed = player.StatsHolder.Stats[StatNames.MOVE_SPEED].Value;
            player.Rigidbody.linearVelocity = move * speed;

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