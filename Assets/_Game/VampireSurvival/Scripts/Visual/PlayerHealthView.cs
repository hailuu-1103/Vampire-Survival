#nullable enable
using System.Collections.Generic;
using Entities_Component = Core.Entities.Component;
using IComponent = Core.Entities.IComponent;
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core.Visual
{
    using UnityEngine;
    using UnityEngine.UI;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Models;

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
            this.UnsubscribeFromPlayer();
        }

        private void OnEntitySpawned(IEntity entity, IReadOnlyList<IComponent> _)
        {
            if (entity is not IPlayer player) return;

            this.player = player;
            this.SubscribeToPlayer();
            this.Refresh();
            this.Manager.Spawned -= this.OnEntitySpawned;
        }

        private void SubscribeToPlayer()
        {
            if (this.player == null) return;

            this.player.StatsHolder.Stats[StatNames.HEALTH].ValueChanged += this.OnStatChanged;
            this.player.StatsHolder.Stats[StatNames.MAX_HEALTH].ValueChanged += this.OnStatChanged;
        }

        private void UnsubscribeFromPlayer()
        {
            if (this.player == null) return;

            this.player.StatsHolder.Stats[StatNames.HEALTH].ValueChanged -= this.OnStatChanged;
            this.player.StatsHolder.Stats[StatNames.MAX_HEALTH].ValueChanged -= this.OnStatChanged;
            this.player = null;
        }

        private void OnStatChanged(float _) => this.Refresh();

        private void Refresh()
        {
            if (this.player == null) return;

            var max = this.player.StatsHolder.Stats[StatNames.MAX_HEALTH].Value;
            var cur = this.player.StatsHolder.Stats[StatNames.HEALTH].Value;
            this.healthFill.fillAmount = max <= 0f ? 0f : Mathf.Clamp01(cur / max);
        }
    }
}