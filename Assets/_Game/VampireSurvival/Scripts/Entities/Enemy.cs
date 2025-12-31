#nullable enable

using Entity = Core.Entities.Entity;

namespace VampireSurvival.Core.Entities
{
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;

    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public sealed class Enemy : Entity, IEnemy
    {
        private IEnemyAnimation anim        = null!;
        private IStatsHolder    statsHolder = null!;
        private Collider2D      col         = null!;
        private Rigidbody2D     rb          = null!;

        protected override void OnInstantiate()
        {
            this.anim        = this.GetComponent<IEnemyAnimation>();
            this.statsHolder = this.GetComponent<IStatsHolder>();
            this.col         = this.GetComponent<Collider2D>();
            this.rb          = this.GetComponent<Rigidbody2D>();
        }

        IEnemyAnimation IEnemy.   Animation   => this.anim;
        IStatsHolder IEnemy.      StatsHolder => this.statsHolder;
        Collider2D IHasCollider.  Collider    => this.col;
        Rigidbody2D IHasRigidbody.Rigidbody   => this.rb;

        protected override void OnSpawn()
        {
            this.anim.SetColor(Color.red);
        }
    }
}