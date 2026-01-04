#nullable enable

using UnityEngine;
using Core.Entities;

namespace VampireSurvival.Systems
{
    using VampireSurvival.Abstractions;
    using VampireSurvival.Models;

    public sealed class PlayerMovementSystem : System<IPlayer>
    {
        protected override bool Filter(IPlayer player)
        {
            return player.IsAlive;
        }

        protected override void Apply(IPlayer player)
        {
            var move = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (move.sqrMagnitude > 1f) move.Normalize();

            var speed = player.StatsHolder.Stats[CharacterStatNames.MOVE_SPEED].Value;
            player.Rigidbody.linearVelocity = move * speed;

            var isMoving = move.sqrMagnitude > 0.01f;

            if (isMoving)
            {
                player.Animation.SetFacing(move.x);
                player.Animation.SetAnimation(AnimationType.Run);
            }
            else
            {
                player.Animation.SetAnimation(AnimationType.Idle);
            }
        }
    }
}