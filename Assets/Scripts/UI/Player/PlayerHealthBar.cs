#nullable enable
using System.Collections.Generic;
using Entities_Component = Core.Entities.Component;
using IComponent = Core.Entities.IComponent;
using IEntity = Core.Entities.IEntity;

namespace Game.UI
{
    using System;
    using System.Linq;
    using Core.DI;
    using Core.Entities;
    using UnityEngine;
    using UnityEngine.UI;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Models;

    public sealed class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthFill = null!;

        private IEntityManager entityManager = null!;
        private IPlayer?       player;

        private void Awake()
        {
            this.entityManager = this.GetCurrentContainer().Resolve<IEntityManager>();
        }

        private void OnEnable()
        {
            this.Refresh();

            this.player                                                      =  this.entityManager.Query<IPlayer>().Single();
            this.player.StatsHolder.Stats[CharacterStatNames.HEALTH].ValueChanged     += this.OnStatChanged;
            this.player.StatsHolder.Stats[CharacterStatNames.MAX_HEALTH].ValueChanged += this.OnStatChanged;
        }

        private void OnDisable()
        {
            if (this.player == null) return;

            this.player.StatsHolder.Stats[CharacterStatNames.HEALTH].ValueChanged     -= this.OnStatChanged;
            this.player.StatsHolder.Stats[CharacterStatNames.MAX_HEALTH].ValueChanged -= this.OnStatChanged;
            this.player                                                      =  null;
        }

        private void OnStatChanged(float _) => this.Refresh();

        private void Refresh()
        {
            if (this.player == null) return;

            var max = this.player.StatsHolder.Stats[CharacterStatNames.MAX_HEALTH].Value;
            var cur = this.player.StatsHolder.Stats[CharacterStatNames.HEALTH].Value;
            this.healthFill.fillAmount = max <= 0f ? 0f : Mathf.Clamp01(cur / max);
        }
    }
}