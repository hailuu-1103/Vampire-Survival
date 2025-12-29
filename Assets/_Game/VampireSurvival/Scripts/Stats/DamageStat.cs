#nullable enable
using Component = Core.Entities.Component;
using IComponent = Core.Entities.IComponent;

namespace VampireSurvival.Core.Stats
{
    using VampireSurvival.Core.Abstractions;

    public interface IDamageStat : IComponent
    {
        public float Value { get; }
        public void  Add(float    delta);
        public void  Multiply(float percent);
    }

    public sealed class DamageStat : Component, IDamageStat
    {
        private IStats stats = null!;

        protected override void OnInstantiate()
        {
            this.stats = this.GetComponent<IStats>();
        }

        public float      Value      => this.stats.Get(StatId.Damage);

        public void Add(float    delta)   => this.stats.Add(StatId.Damage, delta);
        public void Multiply(float percent) => this.stats.Multiply(StatId.Damage, percent);
    }
}