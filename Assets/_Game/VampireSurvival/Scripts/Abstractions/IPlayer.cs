#nullable enable
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core.Abstractions
{
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Core.Stats;

    public interface IPlayer : IEntity
    {
        public IPlayerAnimation Animation  { get; }
        public IPlayerMoveable  Movement   { get; }
        public IStats           Stats      { get; }
        public IHealthStat      HealthStat { get; }
    }

    public interface IPlayerAnimation
    {
        public void    PlayIdleAnimation();
        public void    PlayRunAnimation();
        public void    PlayHitAnimation();
        public void    SetFacing(float direction);
        public UniTask PlayDeathAnimationAsync();
    }
}