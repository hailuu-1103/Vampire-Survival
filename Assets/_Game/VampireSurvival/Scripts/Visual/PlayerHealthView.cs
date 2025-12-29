#nullable enable
using System.Collections.Generic;
using Core.Entities;
using Entities_Component = Core.Entities.Component;

namespace VampireSurvival.Core.UI
{
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Stats;

    public sealed class PlayerHealthView : Entities_Component
    {
        [SerializeField] private Image healthFill = null!;

        private IPlayer? player;

        protected override void OnSpawn()
        {
            this.Manager.Spawned += this.OnEntitySpawned;
        }

        protected override void OnRecycle()
        {
            this.Manager.Spawned -= this.OnEntitySpawned;
            if (this.player == null) return;
            this.player.HealthStat.Changed -= this.OnHealthChanged;
            this.player.Stats.Changed      -= this.OnStatsChanged;
            this.player                    =  null;
        }

        private void OnEntitySpawned(IEntity entity, IReadOnlyList<IComponent> _)
        {
            if (entity is not IPlayer) return;
            this.player                    =  this.Manager.Query<IPlayer>().Single();
            this.Manager.Spawned           -= this.OnEntitySpawned;
            this.player.HealthStat.Changed += this.OnHealthChanged;
            this.player.Stats.Changed      += this.OnStatsChanged;
        }

        private void OnHealthChanged(float current, float max) => this.Refresh();

        private void OnStatsChanged(StatId id, float oldVal, float newVal)
        {
            if (id != StatId.Health) return;
            this.Refresh();
        }

        private void Refresh()
        {
            if (this.player == null) return;
            var max = this.player.HealthStat.Max;
            var cur = this.player.HealthStat.Current;
            this.healthFill.fillAmount = max <= 0f ? 0f : Mathf.Clamp01(cur / max);
        }
    }
}