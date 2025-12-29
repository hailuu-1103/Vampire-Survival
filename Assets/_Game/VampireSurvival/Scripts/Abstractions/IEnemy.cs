#nullable enable
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core.Abstractions
{
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Core.Stats;

    public interface IEnemy : IEntity
    {
        public IEnemyAnimation Animation  { get; }
        public IEnemyMoveable  Movement   { get; }
        public IStats          Stats      { get; }
        public IDamageStat     DamageStat { get; }
        public IHealthStat     HealthStat { get; }
    }

    public interface IEnemyAnimation
    {
        public void    PlayRunAnimation();
        public void    SetFacing(float direction);
        public void    PlayAttackAnimation();
        public UniTask PlayDeathAnimationAsync();
        public UniTask PlayWonAnimationAsync();
    }
}