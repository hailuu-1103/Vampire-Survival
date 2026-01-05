#nullable enable

namespace VampireSurvival.Components
{
    using Component = global::Core.Entities.Component;
    using System.Collections.Generic;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Configs;
    using VampireSurvival.Models;

    public sealed class StatsHolder : Component, IStatsHolder
    {
        [SerializeField] private CharacterStatsConfig config = null!;

        private readonly Dictionary<string, ObservableValue> stats = new();

        IReadOnlyDictionary<string, ObservableValue> IStatsHolder.Stats => this.stats;

        protected override void OnInstantiate()
        {
            this.stats.Clear();

            this.stats[CharacterStatNames.MAX_HEALTH] = new(this.config.maxHealth);
            this.stats[CharacterStatNames.HEALTH]     = new(this.config.maxHealth);
            this.stats[CharacterStatNames.ATTACK]     = new(this.config.attack);
            this.stats[CharacterStatNames.MOVE_SPEED] = new(this.config.moveSpeed);

            this.stats[CharacterStatNames.HEALTH].ValueChanged += this.OnHealthChanged;
        }

        protected override void OnSpawn()
        {
            this.ResetHealth();
        }

        private void ResetHealth()
        {
            if (this.stats.TryGetValue(CharacterStatNames.MAX_HEALTH, out var maxHealth) && this.stats.TryGetValue(CharacterStatNames.HEALTH, out var health))
            {
                health.Value = maxHealth.Value;
            }
        }

        private void OnHealthChanged(float delta)
        {
            if (!this.stats.TryGetValue(CharacterStatNames.HEALTH, out var health)) return;
            if (!this.stats.TryGetValue(CharacterStatNames.MAX_HEALTH, out var maxHealth)) return;

            var clamped = Mathf.Clamp(health.Value, 0f, maxHealth.Value);
            if (!Mathf.Approximately(health.Value, clamped))
            {
                health.Value = clamped;
            }
        }

        public void Add(string name, float value)
        {
            if (this.stats.TryGetValue(name, out var stat))
            {
                stat.Value += value;
            }
            else
            {
                this.stats[name] = new(value);
            }
        }

        public void Remove(string name)
        {
            if (this.stats.TryGetValue(name, out var stat))
            {
                stat.ValueChanged -= this.OnHealthChanged;
            }
            this.stats.Remove(name);
        }

        protected override void OnRecycle()
        {
            this.stats[CharacterStatNames.HEALTH].ValueChanged -= this.OnHealthChanged;
        }
    }
}