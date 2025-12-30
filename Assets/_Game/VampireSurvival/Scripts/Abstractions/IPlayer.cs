#nullable enable
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core.Abstractions
{
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Core.Stats;

    public interface IPlayer : IEntity, IHasCollider
    {
        public IPlayerAnimation Animation  { get; }
        public IPlayerMoveable  Movement   { get; }
        public IStats           Stats      { get; }
        public IHealthStat      HealthStat { get; }
        public IDamageStat      DamageStat { get; }
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