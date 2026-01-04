#nullable enable

namespace VampireSurvival.Entities
{
    using Entity = global::Core.Entities.Entity;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Models;

    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public sealed class Enemy : Entity, IEnemy
    {
        private IAnimation   anim        = null!;
        private IStatsHolder statsHolder = null!;
        private Collider2D   col         = null!;
        private Rigidbody2D  rb          = null!;

        protected override void OnInstantiate()
        {
            this.anim        = this.GetComponent<IAnimation>();
            this.statsHolder = this.GetComponent<IStatsHolder>();
            this.col         = this.GetComponent<Collider2D>();
            this.rb          = this.GetComponent<Rigidbody2D>();
        }

        bool IEnemy.               IsAlive         => this.statsHolder.Stats[CharacterStatNames.HEALTH] > 0;
        IAnimation IEnemy.         Animation       => this.anim;
        IStatsHolder ITarget.      StatsHolder     => this.statsHolder;
        void ITarget.              PlayHitAnim()   => this.anim.SetAnimation(AnimationType.Hit);
        Collider2D IHasCollider.   Collider        => this.col;
        Rigidbody2D IHasRigidbody. Rigidbody       => this.rb;
        OwnerType IAttacker.       OwnerType       => OwnerType.Enemy;
        float IAttacker.           FacingDirection => this.anim.FacingDirection;

        protected override void OnSpawn()
        {
            this.col.isTrigger = false;
            this.anim.SetColor(Color.red);
        }
    }
}