#nullable enable
using Core.Entities;

namespace VampireSurvival.Core
{
    public interface IDamageStat
    {
        float Base  { get; }
        float Value { get; }
    }

    public sealed class DamageStat : Component, IDamageStat
    {
        private PlayerStatsConfig config     = null!;
        private float             multiplier = 1f;

        public float Base  => this.config.Damage;
        public float Value => this.config.Damage * this.multiplier;

        protected override void OnInstantiate()
        {
            var provider = this.GetComponent<IPlayerStatsConfigProvider>();
            this.config = provider.Config;
        }

        protected override void OnSpawn()
        {
            this.multiplier = 1f;
        }

        public void Multiply(float mul) => this.multiplier *= mul;
        public void ResetMultiplier()   => this.multiplier = 1f;
    }
}