#nullable enable
using Entities_Component = Core.Entities.Component;

namespace VampireSurvival.Core.Stats
{
    using UnityEngine;

    public interface IDamageStat
    {
        public GameObject GameObject { get; }
        public float      Base       { get; }
        public float      Value      { get; }
    }

    public sealed class DamageStat : Entities_Component, IDamageStat
    {
        private CharacterBasicStatsConfig config     = null!;
        private float                multiplier = 1f;

        float IDamageStat.     Base       => this.config.Damage;
        float IDamageStat.     Value      => this.config.Damage * this.multiplier;
        GameObject IDamageStat.GameObject => this.gameObject;

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