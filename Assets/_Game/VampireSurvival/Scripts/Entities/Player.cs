#nullable enable

using Entity = Core.Entities.Entity;

namespace VampireSurvival.Core.Entities
{
    using AbilitySystem.Components;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;

    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public sealed class Player : Entity, IPlayer
    {
        public IPlayerAnimation Animation   => this.GetComponent<IPlayerAnimation>();
        public IStatsHolder     StatsHolder => this.GetComponent<IStatsHolder>();
        public Collider2D       Collider    => this.GetComponent<Collider2D>();
        public Rigidbody2D      Rigidbody   => this.GetComponent<Rigidbody2D>();
    }
}