#nullable enable
using Core.Entities;
using UnityEngine;
using Component = Core.Entities.Component;

namespace VampireSurvival.Core.Stats
{
    public interface IHealthStat
    {
        float Max     { get; }
        float Current { get; }
        bool  IsDead  { get; }
        void  Heal(float       amount);
        void  TakeDamage(float amount);
        void  SetToFull();
    }

    public sealed class HealthStat : Component, IHealthStat
    {
        private CharacterBasicStatsConfig config = null!;
        private float                current;

        public float Max     => this.config.MaxHealth;
        public float Current => this.current;
        public bool  IsDead  => this.current <= 0f;

        protected override void OnInstantiate()
        {
            this.config = this.GetComponent<ICharacterStats<CharacterBasicStatsConfig>>().Config;
        }

        protected override void OnSpawn()
        {
            this.SetToFull();
        }

        public void SetToFull()
        {
            this.current = this.Max;
        }

        public void Heal(float amount)
        {
            if (amount <= 0f) return;
            this.current = Mathf.Min(this.Max, this.current + amount);
        }

        public void TakeDamage(float amount)
        {
            if (amount <= 0f) return;
            this.current = Mathf.Max(0f, this.current - amount);
        }
    }
}