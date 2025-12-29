#nullable enable
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core.Abstractions
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VampireSurvival.Core.Stats;

    public interface IEnemy : IEntity, IHasCollider
    {
        public IEnemyAnimation Animation  { get; }
        public IEnemyMoveable  Movement   { get; }
        public IStats          Stats      { get; }
        public IDamageStat     DamageStat { get; }
        public IHealthStat     HealthStat { get; }
    }

    public interface IEnemyAnimation
    {
        public void    SetColor(Color color);
        public void    PlayRunAnimation();
        public void    SetFacing(float direction);
        public void    PlayAttackAnimation();
        public UniTask PlayDeathAnimationAsync();
        public UniTask PlayWonAnimationAsync();
    }
}