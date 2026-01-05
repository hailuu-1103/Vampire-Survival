#nullable enable

using Core.Utils;
using IObjectPoolManager = Core.Pooling.IObjectPoolManager;

namespace VampireSurvival.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using Entity = global::Core.Entities.Entity;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Configs;
    using VampireSurvival.Models;

    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public sealed class Player : Entity, IPlayer
    {
        [SerializeField] private Transform weaponHolder = null!;

        private PlayerConfig playerConfig = null!;
        private IAnimation   anim         = null!;
        private IStatsHolder statsHolder  = null!;
        private Collider2D   col          = null!;

        private Rigidbody2D rb = null!;

        protected override void OnInstantiate()
        {
            this.playerConfig = this.Container.Resolve<PlayerConfig>();
            this.anim         = this.GetComponent<IAnimation>();
            this.statsHolder  = this.GetComponent<IStatsHolder>();
            this.col          = this.GetComponent<Collider2D>();
            this.rb           = this.GetComponent<Rigidbody2D>();
        }

        bool IPlayer.             IsAlive         => this.statsHolder.Stats[CharacterStatNames.HEALTH] > 0;
        IAnimation IPlayer.       Animation       => this.anim;
        IStatsHolder ITarget.     StatsHolder     => this.statsHolder;
        void ITarget.             PlayHitAnim()   => this.anim.SetAnimation(AnimationType.Hit);
        Collider2D IHasCollider.  Collider        => this.col;
        Rigidbody2D IHasRigidbody.Rigidbody       => this.rb;
        bool IImmortalable.       IsImmortal      => this.isImmortal;
        OwnerType IAttacker.      OwnerType       => OwnerType.Player;
        float IAttacker.          FacingDirection => this.anim.FacingDirection;

        void IImmortalable.SetImmortal(bool immortal) => this.isImmortal = immortal;

        public  IReadOnlyCollection<IWeapon> Weapons => this.weapons;
        private HashSet<IWeapon>             weapons = new();
        private bool                         isImmortal;

        protected override void OnSpawn()
        {
            this.weapons = this.playerConfig.InitialWeapons
                .Select(prefab => (IWeapon)this.Manager.Spawn(prefab, parent: this.weaponHolder))
                .ToHashSet();
        }
    }
}