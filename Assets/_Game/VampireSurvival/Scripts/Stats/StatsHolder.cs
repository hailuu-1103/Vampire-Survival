#nullable enable

namespace VampireSurvival.Core.Stats
{
    using Component = global::Core.Entities.Component;
    using System.Collections.Generic;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Models;

    public sealed class StatsHolder : Component, IStatsHolder
    {
        [SerializeField] private CharacterStatsConfig config = null!;

        private readonly Dictionary<string, ObservableValue> stats = new();

        public IReadOnlyDictionary<string, ObservableValue> Stats => this.stats;

        protected override void OnInstantiate()
        {
            this.stats.Clear();
            this.InitializeFromConfig();
        }

        protected override void OnSpawn()
        {
            this.ResetStats();
        }

        private void InitializeFromConfig()
        {
            this.stats[StatNames.MAX_HEALTH]      = new(this.config.maxHealth);
            this.stats[StatNames.HEALTH]          = new(this.config.maxHealth);
            this.stats[StatNames.ATTACK]          = new(this.config.attack);
            this.stats[StatNames.ATTACK_RANGE]    = new(this.config.attackRange);
            this.stats[StatNames.ATTACK_COOLDOWN] = new(this.config.attackCooldown);
            this.stats[StatNames.MOVE_SPEED]      = new(this.config.moveSpeed);

            this.stats[StatNames.HEALTH].ValueChanged += this.OnHealthChanged;
        }

        private void ResetStats()
        {
            if (this.stats.TryGetValue(StatNames.MAX_HEALTH, out var maxHealth) &&
                this.stats.TryGetValue(StatNames.HEALTH, out var health))
            {
                health.Value = maxHealth.Value;
            }
        }

        private void OnHealthChanged(float delta)
        {
            if (!this.stats.TryGetValue(StatNames.HEALTH, out var health)) return;
            if (!this.stats.TryGetValue(StatNames.MAX_HEALTH, out var maxHealth)) return;

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
            if (this.stats.TryGetValue(StatNames.HEALTH, out var healthStat))
            {
                healthStat.ValueChanged -= this.OnHealthChanged;
            }
        }
    }
}