#nullable enable

using Entity = Core.Entities.Entity;

namespace VampireSurvival.Core.Entities
{
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;

    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public sealed class Player : Entity, IPlayer
    {
        private IPlayerAnimation anim        = null!;
        private IStatsHolder     statsHolder = null!;
        private Collider2D       col         = null!;
        private Rigidbody2D      rb          = null!;
        private bool             isImmortal;

        protected override void OnInstantiate()
        {
            this.anim        = this.GetComponent<IPlayerAnimation>();
            this.statsHolder = this.GetComponent<IStatsHolder>();
            this.col         = this.GetComponent<Collider2D>();
            this.rb          = this.GetComponent<Rigidbody2D>();
        }

        IPlayerAnimation IPlayer. Animation   => this.anim;
        IStatsHolder IPlayer.     StatsHolder => this.statsHolder;
        Collider2D IHasCollider.  Collider    => this.col;
        Rigidbody2D IHasRigidbody.Rigidbody   => this.rb;
        bool IImmortalable.       IsImmortal  => this.isImmortal;

        void IImmortalable.SetImmortal(bool immortal)
        {
            this.isImmortal = immortal;
        }
    }
}