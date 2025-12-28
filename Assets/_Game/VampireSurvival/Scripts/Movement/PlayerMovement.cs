#nullable enable
using UnityEngine;
using Component = Core.Entities.Component;

namespace VampireSurvival.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerMovement : Component, IMoveable
    {
        private Rigidbody2D rb        = null!;
        private IMoveStat   moveSpeed = null!;
        private Vector2     moveInput;

        protected override void OnInstantiate()
        {
            this.rb        = this.GetComponent<Rigidbody2D>();
            this.moveSpeed = this.GetComponent<IMoveStat>();
        }

        void IMoveable.Move(Vector2 move)
        {
            this.moveInput = move;
        }

        private void FixedUpdate()
        {
            var speed = this.moveSpeed.Value;
            var next  = this.rb.position + this.moveInput * speed * Time.fixedDeltaTime;
            this.rb.MovePosition(next);
        }
    }
}