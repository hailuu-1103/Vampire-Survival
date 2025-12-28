#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using Component = Core.Entities.Component;

namespace VampireSurvival.Core.Stats
{
    using VampireSurvival.Core.Abstractions;

    public sealed class StatsContainer : Component, IStats
    {
        [SerializeField] private CharacterStatsConfig config = null!;

        private readonly Dictionary<StatId, StatRuntime> stats = new();

        public event Action<StatId, float, float>? Changed;

        protected override void OnInstantiate()
        {
            this.stats.Clear();

            foreach (StatId id in Enum.GetValues(typeof(StatId)))
            {
                var baseValue = this.config.TryGet(id, out var v) ? v : 0f;
                this.stats[id] = new(baseValue);
            }
        }

        protected override void OnSpawn()
        {
            foreach (var kv in this.stats)
            {
                kv.Value.ResetModifiers();
            }
        }

        public float Get(StatId id)
            => this.stats.TryGetValue(id, out var s) ? s.Value : 0f;

        public void Add(StatId id, float delta)
        {
            if (!this.stats.TryGetValue(id, out var s)) return;

            var old = s.Value;
            s.flatBonus += delta;
            var @new = s.Value;

            if (!Mathf.Approximately(old, @new))
                this.Changed?.Invoke(id, old, @new);
        }

        public void Multiply(StatId id, float delta)
        {
            if (!this.stats.TryGetValue(id, out var s)) return;

            var old = s.Value;
            s.multiplier = Mathf.Max(0f, s.multiplier + delta);
            var @new = s.Value;

            if (!Mathf.Approximately(old, @new))
                this.Changed?.Invoke(id, old, @new);
        }

        public void ResetModifiers()
        {
            foreach (var kv in this.stats)
            {
                var old = kv.Value.Value;
                kv.Value.ResetModifiers();
                var @new = kv.Value.Value;

                if (!Mathf.Approximately(old, @new))
                    this.Changed?.Invoke(kv.Key, old, @new);
            }
        }

        private sealed class StatRuntime
        {
            public readonly float baseValue;
            public float flatBonus;
            public float multiplier = 1f;

            public StatRuntime(float baseValue) => this.baseValue = baseValue;

            public float Value => (this.baseValue + this.flatBonus) * this.multiplier;

            public void ResetModifiers()
            {
                this.flatBonus = 0f;
                this.multiplier = 1f;
            }
        }
    }
}