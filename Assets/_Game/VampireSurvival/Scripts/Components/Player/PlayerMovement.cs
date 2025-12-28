#nullable enable
using UnityEngine;
using Component = Core.Entities.Component;

namespace VampireSurvival.Core.Components
{
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Stats;

    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerMovement : Component, IPlayerMoveable
    {
        private Rigidbody2D    rb        = null!;
        private IMoveSpeedStat moveSpeed = null!;
        private Vector2        moveInput;

        protected override void OnInstantiate()
        {
            this.rb        = this.GetComponent<Rigidbody2D>();
            this.moveSpeed = this.GetComponent<IMoveSpeedStat>();
        }

        void IMoveable.Move(Vector2 move)
        {
            this.moveInput = move;
        }

        Vector2 IMoveable.Position => this.transform.position;

        private void FixedUpdate()
        {
            var speed = this.moveSpeed.Value;
            var next  = this.rb.position + this.moveInput * speed * Time.fixedDeltaTime;
            this.rb.MovePosition(next);
        }
    }
}