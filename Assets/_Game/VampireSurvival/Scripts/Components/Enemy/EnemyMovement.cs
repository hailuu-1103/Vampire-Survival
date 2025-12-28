#nullable enable
using UnityEngine;
using Component = Core.Entities.Component;
using VampireSurvival.Core.Stats;

namespace VampireSurvival.Core.Enemies
{
    using VampireSurvival.Core.Abstractions;

    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class EnemyMovement : Component, IEnemyMoveable
    {
        private Rigidbody2D    rb        = null!;
        private IMoveSpeedStat moveSpeed = null!;
        private Vector2        moveInput;

        public Vector2 Position => this.rb.position;

        protected override void OnInstantiate()
        {
            this.rb        = this.GetComponent<Rigidbody2D>();
            this.moveSpeed = this.GetComponent<IMoveSpeedStat>();
        }

        public void Move(Vector2 move)
        {
            this.moveInput = move;
        }

        private void FixedUpdate()
        {
            var next = this.rb.position + this.moveInput * this.moveSpeed.Value * Time.fixedDeltaTime;
            this.rb.MovePosition(next);
        }
    }
}