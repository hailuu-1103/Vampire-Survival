#nullable enable
using System;
using UnityEngine;
using Component = Core.Entities.Component;
using IComponent = Core.Entities.IComponent;

namespace VampireSurvival.Core.Stats
{
    using VampireSurvival.Core.Abstractions;

    public interface IHealthStat : IComponent
    {
        public event Action<float, float> Changed;
        public event Action               Died;

        public float Current { get; }
        public float Max     { get; }

        public void TakeDamage(float amount);
        //TODO
        public void Heal(float       amount);
        //TODO
        public void Refill(float delta, bool healToFull);
    }

    public sealed class HealthStat : Component, IHealthStat
    {
        private IStats stats = null!;
        private float  current;

        public event Action<float, float>? Changed;
        public event Action?               Died;

        protected override void OnInstantiate()
        {
            this.stats         =  this.GetComponent<IStats>();
        }

        protected override void OnSpawn()
        {
            this.current = this.Max;
            this.stats.Changed += this.OnAnyStatChanged;
            this.Changed?.Invoke(this.current, this.Max);
        }

        protected override void OnRecycle()
        {
            this.stats.Changed -= this.OnAnyStatChanged;
        }

        public float Max     => this.stats.Get(StatId.Health);
        public float Current => this.current;

        public void TakeDamage(float amount)
        {
            if (amount <= 0f || this.current <= 0f) return;
            this.current = Mathf.Max(0f, this.current - amount);
            this.Changed?.Invoke(this.current, this.Max);

            if (this.current <= 0f)
            {
                this.Died?.Invoke();
            }
        }

        public void Heal(float amount)
        {
            if (amount <= 0f) return;

            this.current = Mathf.Min(this.Max, this.current + amount);
            this.Changed?.Invoke(this.current, this.Max);
        }

        public void Refill(float delta, bool healToFull)
        {
            if (delta <= 0f) return;

            this.stats.Add(StatId.Health, delta);
            if (healToFull) this.current = this.Max;

            this.Changed?.Invoke(this.current, this.Max);
        }

        private void OnAnyStatChanged(StatId id, float oldValue, float newValue)
        {
            if (id != StatId.Health) return;

            this.current = Mathf.Min(this.current, this.Max);
            this.Changed?.Invoke(this.current, this.Max);
        }
    }
}