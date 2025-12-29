#nullable enable

using Entities_Component = Core.Entities.Component;
using IEventBus = Core.Observer.IEventBus;

namespace VampireSurvival.Core.Components
{
    using UnityEngine;
    using VampireSurvival.Core.Entities;
    using VampireSurvival.Core.Events;
    using VampireSurvival.Core.UI;

    public sealed class GameManager : Entities_Component
    {
        [SerializeField] private GameCanvas        gameCanvas        = null!;
        [SerializeField] private GameSystemsRunner gameSystemsRunner = null!;
        [SerializeField] private Tutorial          tutorial          = null!;
        [SerializeField] private Player            player            = null!;
        [SerializeField] private Enemy             enemy             = null!;

        public GameCanvas        GameCanvas        => this.gameCanvas;
        public Tutorial          Tutorial          => this.tutorial;
        public GameSystemsRunner GameSystemsRunner => this.gameSystemsRunner;
        public Player            Player            => this.player;
        public Enemy             Enemy             => this.enemy;

        private IEventBus eventBus = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
        }

        protected override void OnSpawn()
        {
            this.player.HealthStat.Died += this.OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            this.eventBus.Publish<LostEvent>(new());
        }

        protected override void OnRecycle()
        {
            this.player.HealthStat.Died -= this.OnPlayerDied;
        }
    }
}