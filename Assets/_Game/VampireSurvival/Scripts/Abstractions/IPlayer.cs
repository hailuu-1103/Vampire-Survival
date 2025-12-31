#nullable enable
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core.Abstractions
{
    using AbilitySystem.Components;
    using Cysharp.Threading.Tasks;

    public interface IPlayer : IEntity, IHasCollider, IHasRigidbody
    {
        public IPlayerAnimation Animation   { get; }
        public IStatsHolder     StatsHolder { get; }
    }

    public interface IPlayerAnimation
    {
        public bool    CanMove         { get; }
        public float   FacingDirection { get; }
        public void    PlayIdleAnimation();
        public void    PlayRunAnimation();
        public void    PlayHitAnimation();
        public void    PlayAttackAnimation();
        public void    SetFacing(float direction);
        public UniTask PlayDeathAnimationAsync();
    }
}