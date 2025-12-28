#nullable enable
using Core.Entities;

namespace VampireSurvival.Core.Stats
{
    public interface IMoveStat
    {
        float Base  { get; }
        float Value { get; }
    }

    public sealed class MoveStat : Component, IMoveStat
    {
        private CharacterBasicStatsConfig config     = null!;
        private float                multiplier = 1f;

        public float Base  => this.config.MoveSpeed;
        public float Value => this.config.MoveSpeed * this.multiplier;

        protected override void OnInstantiate()
        {
            this.config = this.GetComponent<ICharacterStats<CharacterBasicStatsConfig>>().Config;
        }

        protected override void OnSpawn()
        {
            this.multiplier = 1f;
        }

        public void Multiply(float mul) => this.multiplier *= mul;
        public void ResetMultiplier()   => this.multiplier = 1f;
    }
}