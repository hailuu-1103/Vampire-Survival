#nullable enable
using Component = Core.Entities.Component;
using IComponent = Core.Entities.IComponent;

namespace VampireSurvival.Core.Stats
{
    using VampireSurvival.Core.Abstractions;

    public interface IMoveSpeedStat : IComponent
    {
        public float Value { get; }
    }

    public sealed class MoveStat : Component, IMoveSpeedStat
    {
        private IStats stats = null!;

        protected override void OnInstantiate()
        {
            this.stats = this.GetComponent<IStats>();
        }

        public float Value => this.stats.Get(StatId.MoveSpeed);

        public void AddPercent(float percent)
            => this.stats.Multiply(StatId.MoveSpeed, percent);
    }
}