#nullable enable
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core.Abstractions
{
    using AbilitySystem.Components;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public interface IEnemy : IEntity, IHasCollider, IHasRigidbody
    {
        public IEnemyAnimation Animation   { get; }
        public IStatsHolder    StatsHolder { get; }
    }

    public interface IEnemyAnimation
    {
        public bool    CanMove         { get; }
        public float   FacingDirection { get; }
        public void    SetColor(Color color);
        public void    PlayRunAnimation();
        public void    SetFacing(float direction);
        public void    PlayAttackAnimation();
        public void    PlayHitAnimation();
        public void    Stop();
        public UniTask PlayDeathAnimationAsync();
        public UniTask PlayWonAnimationAsync();
    }
}
