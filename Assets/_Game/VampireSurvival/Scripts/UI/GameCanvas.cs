#nullable enable
using Entity = Core.Entities.Entity;

namespace VampireSurvival.Core.UI
{
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Stats;

    public sealed class GameCanvas : Entity
    {
        [SerializeField] private GameObject gameOverPanel = null!;
        [SerializeField] private Image      healthFill    = null!;

        private IPlayer? player;

        protected override void OnSpawn()
        {
            this.gameOverPanel.SetActive(false);
            this.player                    =  this.Manager.Query<IPlayer>().Single();
            this.player.HealthStat.Changed += this.OnHealthChanged;
            this.player.Stats.Changed      += this.OnStatsChanged;
            this.player.HealthStat.Died    += this.OnPlayerDied;
        }

        protected override void OnRecycle()
        {
            if (this.player == null) return;
            this.player!.HealthStat.Changed -= this.OnHealthChanged;
            this.player.Stats.Changed       -= this.OnStatsChanged;
            this.player.HealthStat.Died     -= this.OnPlayerDied;
            this.player                     =  null;
        }

        private void OnPlayerDied()
        {
            this.gameOverPanel.SetActive(true);
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