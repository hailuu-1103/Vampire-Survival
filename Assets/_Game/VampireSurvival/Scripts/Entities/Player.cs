#nullable enable

namespace VampireSurvival.Entities
{
    using Entity = global::Core.Entities.Entity;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Models;

    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public sealed class Player : Entity, IPlayer
    {
        private IAnimation   anim        = null!;
        private IStatsHolder statsHolder = null!;
        private Collider2D   col         = null!;
        private Rigidbody2D  rb          = null!;
        private bool         isImmortal;

        protected override void OnInstantiate()
        {
            this.anim        = this.GetComponent<IAnimation>();
            this.statsHolder = this.GetComponent<IStatsHolder>();
            this.col         = this.GetComponent<Collider2D>();
            this.rb          = this.GetComponent<Rigidbody2D>();
        }

        bool IPlayer.              IsAlive         => this.statsHolder.Stats[CharacterStatNames.HEALTH] > 0;
        IAnimation IPlayer.        Animation       => this.anim;
        IStatsHolder ITarget.      StatsHolder     => this.statsHolder;
        void ITarget.              PlayHitAnim()   => this.anim.SetAnimation(AnimationType.Hit);
        Collider2D IHasCollider.   Collider        => this.col;
        Rigidbody2D IHasRigidbody. Rigidbody       => this.rb;
        bool IImmortalable.        IsImmortal      => this.isImmortal;
        OwnerType IAttacker.       OwnerType       => OwnerType.Player;
        float IAttacker.           FacingDirection => this.anim.FacingDirection;

        void IImmortalable.SetImmortal(bool immortal) => this.isImmortal = immortal;
    }
}