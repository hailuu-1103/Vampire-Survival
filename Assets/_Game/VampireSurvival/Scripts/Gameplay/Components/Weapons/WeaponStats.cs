#nullable enable

namespace VampireSurvival.Components
{
    using System.Collections.Generic;
    using Component = global::Core.Entities.Component;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Models;

    public sealed class WeaponStats : Component, IStatsHolder
    {
        private readonly Dictionary<string, ObservableValue> stats = new();

        IReadOnlyDictionary<string, ObservableValue> IStatsHolder.Stats => this.stats;

        protected override void OnSpawn()
        {
            this.stats.Clear();
            this.stats[WeaponStatNames.DAMAGE]   = new(0f);
            this.stats[WeaponStatNames.COOLDOWN] = new(1f);
        }

        public void Add(string name, float value)
        {
            if (this.stats.TryGetValue(name, out var stat))
                stat.Value += value;
            else
                this.stats[name] = new(value);
        }

        public void Remove(string name) => this.stats.Remove(name);
    }
}